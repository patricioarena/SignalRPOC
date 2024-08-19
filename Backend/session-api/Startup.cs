using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using session_api.Core;
using session_api.IService;
using session_api.Model;
using session_api.Service;
using session_api.Signal;
using System;
using System.IO;
using System.Reflection;

namespace session_api
{
    public class Startup
    {
        private readonly string AllowAll = "_allowAll";
        private readonly ILogger _logger;

        private static OpenApiContact contact = new OpenApiContact { Email = "patricio.e.arena@gmail.com", Name = "Patricio Ernesto Antonio Arena" };
        private static OpenApiInfo Info = new OpenApiInfo { Title = "Session Api", Version = "v1", Contact = contact };

        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration, ILogger<Startup> logger, IHostEnvironment env)
        {
            Configuration = configuration;
            _logger = logger;

            var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "ASP0000:Do not call 'IServiceCollection.BuildServiceProvider' in 'ConfigureServices'", Justification = "<pendiente>")]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(ILogger), services.BuildServiceProvider().GetService<ILogger<Startup>>());
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ILoggic, Loggic>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUrlConnectionService, UrlConnectionService>();
            services.AddSingleton<IConnectionUserService, ConnectionUserService>();

            // Registra todos los validadores en el ensamblado
            services.AddValidatorsFromAssemblyContaining<Payload>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
            });

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Info.Version, Info);
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = false;
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowAll,
                    builder =>
                    {
                        builder
                        .SetIsOriginAllowed(origin => true) // allow any origin
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        ;
                    });
            });

            _logger.LogInformation("Added services");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                _logger.LogInformation("In Development environment");
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{Info.Version}/swagger.json", $"{Info.Title} {Info.Version}");
                c.RoutePrefix = "swagger";
            });

            var rewrite = new RewriteOptions().AddRedirect("^$", "swagger");
            app.UseRewriter(rewrite);

            app.UseRouting();
            app.UseCors(AllowAll);
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<SignalHub>("/SignalHub").RequireCors(AllowAll);
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                    .RequireCors(AllowAll);
            });
        }
    }
}
