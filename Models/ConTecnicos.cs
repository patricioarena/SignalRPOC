using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_api.Models
{
    [Table("ConocimientosTecnicos", Schema = "dbo")]
    public class ConTecnicos
    {
        [Key, Column("ConTecnicosID")]
        public Guid ConTecnicosID { get; set; }

        [Required, Column("Titulo"), Display(Name = "Titulo"), EnumDataType(typeof(Titulo))]
        public Titulo Titulo{ get; set; }

        [Required, StringLength(50), Column("Conocimiento"), Display(Name = "Conocimiento"), DataType(DataType.Text)]
        public string Conocimiento { get; set; }

        [Required, Column("Nivel"), Display(Name = "Nivel"), EnumDataType(typeof(Nivel))]
        public Nivel Nivel { get; set; }

        [ForeignKey("PersonaID")]
        public DatosPersonales datosPersonales { get; set; }
        public Guid PersonaID { get; set; }
    }

    public enum Titulo
    {
        Lenguajes_de_programación = 1,
        Sistemas_operativos = 2,
        Redes_conectividad_e_Internet = 3,
        Bases_de_Datos = 4,
        Hardware = 5,
        Aplicaciones_Laborales = 6
    }

}
