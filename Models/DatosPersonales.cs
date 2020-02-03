using aspnet_core_api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnet_core_api.Models
{
    [Table("DatosPersonales", Schema = "dbo")]
    public class DatosPersonales
    {
        [Key, Column("PersonaID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PersonaID { get; set; }

        [Required, StringLength(50), Column("Nombre"), DataType(DataType.Text)]
        public string Nombre { get; set; }

        [Required, StringLength(50), Column("Apellido"), DataType(DataType.Text)]
        public string Apellido { get; set; }

        [Required, Column("FechaDeNac"), Display(Name = "Fecha de nacimiento"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaDeNac { get; set; }

        [Required, Column("TEL"), Display(Name = "Tel. Fijo"), DataType(DataType.PhoneNumber)]
        public int TEL { get; set; }

        [Required, Column("CEL"), Display(Name = "Tel. Movil"), DataType(DataType.PhoneNumber)]
        public int CEL { get; set; }

        [Required, StringLength(50), Column("Email"), Display(Name = "Correo electronico"), DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email no valido.")]
        public string Email { get; set; }

        [Required, StringLength(50), Column("Domicilio"), Display(Name = "Domicilio"), DataType(DataType.Custom)]
        public virtual Domicilio Domicilio { get; set; }

        [Column("Experiencias"), Display(Name = "Experiencias"), ForeignKey("ExperienciaID")]
        public IEnumerable<Experiencia> Experiencias { get; set; }

        [Column("Estudios"), Display(Name = "Estudios"), ForeignKey("EstudiosID")]
        public IEnumerable<Estudio> Estudios { get; set; }

        [Column("Idiomas"), Display(Name = "Idiomas"), ForeignKey("IdiomasID")]
        public IEnumerable<Lenguaje> Idiomas { get; set; }

        [Column("ConTecnicos"), Display(Name = "Conocimientos Tecnicos"), ForeignKey("ConAdicionalesID")]
        public IEnumerable<ConTecnicos> ConocimientosTecnicos { get; set; }

        [Column("ConAdicioneles"), Display(Name = "Conocimientos Adicionales"), ForeignKey("ConAdicionalesID")]
        public IEnumerable<ConAdicioneles> ConocimientosAdicionales { get; set; }
    }
}
