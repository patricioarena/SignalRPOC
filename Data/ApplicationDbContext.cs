using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using aspnet_core_api.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace aspnet_core_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public string _ConnectionString { get; set; }
        public ApplicationDbContext(string connectionString) : base() { _ConnectionString = connectionString; }

        public ApplicationDbContext() : base() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_ConnectionString);
                //optionsBuilder.UseSqlServer(_ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<DatosPersonales> DatosPersonales { get; set; }
        public DbSet<Domicilio> Domicilio { get; set; }
        public DbSet<Experiencia> Experiencia { get; set; }
        public DbSet<Estudio> Estudio { get; set; }
        public DbSet<Lenguaje> Idiomas { get; set; }
        public DbSet<ConAdicioneles> ConocimientosAdicionales { get; set; }
        public DbSet<ConTecnicos> ConocimientosTecnicos { get; set; }



    }



}
