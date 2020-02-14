using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_api.Models
{
    [Table("ConocimientosAdicionales", Schema = "dbo")]
    public class ConAdicioneles
    {
        [Key, Column("ConAdicionalesID")]
        public Guid ConAdicionalesID { get; set; }

        [Required, StringLength(20), Column("Titulo"), Display(Name = "Titulo"), DataType(DataType.Text)]
        public string Titulo { get; set; }

        [Required, StringLength(254), Column("Descripcion"), Display(Name = "Descripcion"), DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [ForeignKey("PersonaID")]
        public DatosPersonales datosPersonales { get; set; }
        public Guid PersonaID { get; set; }
    }
}