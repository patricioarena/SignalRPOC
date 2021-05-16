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

        [Required, Column("TEL"), Display(Name = "Tel. Fijo")]
        public string TEL { get; set; }

        [Required, Column("CEL"), Display(Name = "Tel. Movil")]
        public string CEL { get; set; }

        [Required, StringLength(50), Column("Email"), Display(Name = "Correo electronico"), DataType(DataType.EmailAddress)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email no valido.")]
        public string Email { get; set; }

        [InverseProperty("Domicilios")]
        [Required, Column("Domicilios"), Display(Name = "Domicilios")]
        public ICollection<Domicilio> Domicilios { get; set; }

        [InverseProperty("Estudios")]
        [Column("Estudios"), Display(Name = "Estudios")]
        public ICollection<Estudio> Estudios { get; set; }

        [InverseProperty("Idiomas")]
        [Column("Idiomas"), Display(Name = "Idiomas")]
        public ICollection<Lenguaje> Idiomas { get; set; }

        [InverseProperty("Experiencias")]
        [Column("Experiencias"), Display(Name = "Experiencias")]
        public ICollection<Experiencia> Experiencias { get; set; }

        [InverseProperty("ConocimientosTecnicos")]
        [Column("ConTecnicos"), Display(Name = "Conocimientos Tecnicos")]
        public ICollection<ConTecnicos> ConocimientosTecnicos { get; set; }

        [InverseProperty("ConocimientosAdicionales")]
        [Column("ConAdicioneles"), Display(Name = "Conocimientos Adicionales")]
        public ICollection<ConAdicioneles> ConocimientosAdicionales { get; set; }

    }
}
