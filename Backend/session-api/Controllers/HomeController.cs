using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using session_api.Model;
using System.Diagnostics;

namespace session_api.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor _HttpContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor HttpContext)
        {
            _HttpContext = HttpContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Log message in the Index() method");
            return View();
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Log message in the Privacy() method");
            return View();
        }

        public IActionResult Swagger()
        {
            var baseUrl = GetBaseUrl();
            var swaggerUrl = $"{baseUrl}/swagger/index.html";
            _logger.LogInformation($"Log message in the Swagger() ::> [ Redirect ] {swaggerUrl}");
            return Redirect(swaggerUrl);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            _logger.LogError("Log message in the Error() method");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GetBaseUrl()
        {
            var scheme = _HttpContext.HttpContext.Request.Scheme;
            var host = _HttpContext.HttpContext.Request.Host;
            return scheme + "://" + host;
        }
    }
}
