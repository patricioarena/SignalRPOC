using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using aspnet_core_api.Models;
using System.ComponentModel.DataAnnotations;

namespace aspnet_core_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
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
