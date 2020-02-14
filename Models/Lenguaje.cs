using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_api.Models
{
    [Table("Idiomas", Schema = "dbo")]
    public class Lenguaje
    {
        [Key, Column("IdiomasID")]
        public Guid IdiomasID { get; set; }

        [Required, StringLength(20), Column("Idioma"), Display(Name = "Idioma"), DataType(DataType.Text)]
        public string Idioma { get; set; }

        [Required, Column("NivelEscrito"), Display(Name = "Nivel Escrito"), EnumDataType(typeof(Nivel))]
        public Nivel NivelEscrito { get; set; }

        [Required, Column("NivelLectura"), Display(Name = "Nivel Lectura"), EnumDataType(typeof(Nivel))]
        public Nivel NivelLectura { get; set; }

        [Required, Column("NivelOral"), Display(Name = "Nivel Oral"), EnumDataType(typeof(Nivel))]
        public Nivel NivelOral { get; set; }

        [ForeignKey("PersonaID")]
        public DatosPersonales datosPersonales { get; set; }
        public Guid PersonaID { get; set; }
    }
}