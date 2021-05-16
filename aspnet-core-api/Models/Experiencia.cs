using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_api.Models
{
    [Table("Experiencia", Schema = "dbo")]
    public class Experiencia
    {
        [Key, Column("ExperienciaID")]
        public Guid ExperienciaID { get; set; }

        [Required, Column("FechaDeInicio"), Display(Name = "Fecha de inicio"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaDeInicio { get; set; }

        [Column("FechaDeFin"), Display(Name = "Fecha de fin"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaDeFin { get; set; }

        [Required, StringLength(50), Column("Cargo"), DataType(DataType.Text)]
        public string Cargo { get; set; }

        [Required, StringLength(50), Column("TipoDeEmpleo"), Display(Name = "Tipo de empleo"), DataType(DataType.Text)]
        public string TipoDeEmpleo { get; set; }

        [Required, StringLength(50), Column("Empresa"), Display(Name = "Empresa"), DataType(DataType.Text)]
        public string Empresa { get; set; }

        [StringLength(50), Column("Ubicacion"), Display(Name = "Ubicación"), DataType(DataType.Text)]
        public string Ubicacion { get; set; }

        [StringLength(254), Column("Descripcion"), Display(Name = "Descripción"), DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }




        [ForeignKey("Experiencias")]
        public Guid PersonaID { get; set; }
        public DatosPersonales Experiencias { get; set; }

    }
}