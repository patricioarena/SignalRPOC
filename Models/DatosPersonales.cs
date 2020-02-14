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

        //[InverseProperty("datosPersonales")]
        //[Column("Experiencias"), Display(Name = "Experiencias")]
        //public ICollection<Experiencia> Experiencias { get; set; }

        //[InverseProperty("datosPersonales")]
        //[Column("Estudios"), Display(Name = "Estudios")]
        //public ICollection<Estudio> Estudios { get; set; }

        //[InverseProperty("datosPersonales")]
        //[Column("Idiomas"), Display(Name = "Idiomas")]
        //public ICollection<Lenguaje> Idiomas { get; set; }

        //[InverseProperty("datosPersonales")]
        //[Column("ConTecnicos"), Display(Name = "Conocimientos Tecnicos")]
        //public ICollection<ConTecnicos> ConocimientosTecnicos { get; set; }

        //[InverseProperty("datosPersonales")]
        //[Column("ConAdicioneles"), Display(Name = "Conocimientos Adicionales")]
        //public ICollection<ConAdicioneles> ConocimientosAdicionales { get; set; }



        //[InverseProperty("datosPersonales")]
        //[Required, StringLength(50), Column("Domicilio"), Display(Name = "Domicilio"), DataType(DataType.Custom)]
        //public Guid DomicilioID { get; set; }
        //public Domicilio Domicilio { get; set; }


        [ForeignKey("Domicilio")]
        [Required, StringLength(50), Column("DomicilioID"), Display(Name = "DomicilioID"), DataType(DataType.Custom)]
        public Guid DomicilioID { get; set; }
        public Domicilio Domicilio { get; set; }

    }



    //public class Teacher
    //{
    //    public int TeacherId { get; set; }
    //    public string Name { get; set; }

    //    [InverseProperty("OnlineTeacher")]
    //    public ICollection<Course> OnlineCourses { get; set; }

    //}

    //public class Course
    //{
    //    public int CourseId { get; set; }
    //    public string CourseName { get; set; }
    //    public string Description { get; set; }

    //    [ForeignKey("OnlineTeacher")]
    //    public int? OnlineTeacherId { get; set; }
    //    public Teacher OnlineTeacher { get; set; }

    //}

}
