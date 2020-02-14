using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_api.Models
{
    [Table("Estudio", Schema = "dbo")]
    public class Estudio
    {
        [Key, Column("EstudioID")]
        public Guid EstudioID { get; set; }

        [Required, StringLength(50), Column("Establecimiento"), Display(Name = "Establecimiento"), DataType(DataType.Text)]
        public string Establecimiento { get; set; }

        [Required, StringLength(50), Column("Titulo"), Display(Name = "Titulo"), DataType(DataType.Text)]
        public string Titulo { get; set; }

        [Required, StringLength(50), Column("Disciplina"), Display(Name = "Disciplina académica"), DataType(DataType.Text)]
        public string Disciplina { get; set; }

        [Required, Column("FechaDeInicio"), Display(Name = "Fecha de inicio"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaDeInicio { get; set; }

        [Column("FechaDeFin"), Display(Name = "Fecha de fin"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaDeFin { get; set; }

        [Required, StringLength(50), Column("ActExtra"), Display(Name = "Actividades extra curriculares"), DataType(DataType.Text)]
        public string ActExtra { get; set; }

        [Required, StringLength(254), Column("Descripcion"), Display(Name = "Descripción"), DataType(DataType.MultilineText)]
        public string Descripcion { get; set; }

        [ForeignKey("PersonaID")]
        public DatosPersonales datosPersonales { get; set; }
        public Guid PersonaID { get; set; }
    }
}