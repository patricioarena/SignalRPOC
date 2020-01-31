using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace aspnet_core_api.Data
{
    [Table("DatosPersonales", Schema = "dbo")]
    public class DatosPersonales
    {
        [Key]
        [Column("PersonaID")]
        public Guid PersonaID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Apellido")]
        public string Apellido { get; set; }

        [Required]
        [Column("FechaDeNac")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaDeNac { get; set; }

        [Required]
        [Column("TEL")]
        [Display(Name = "Tel. Fijo")]
        public int TEL { get; set; }

        [Required]
        [Column("CEL")]
        [Display(Name = "Tel. Movil")]
        public int CEL { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Email")]
        [Display(Name = "Correo electronico")]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Domicilio")]
        [Display(Name = "Domicilio")]
        [ForeignKey("DomicilioID")]
        public Domicilio Domicilio { get; set; }

        [Column("Experiencias")]
        [Display(Name = "Experiencias")]
        [ForeignKey("ExperienciaID")]
        public IEnumerable<Experiencia> Experiencias { get; set; }

        [Column("Estudios")]
        [Display(Name = "Estudios")]
        [ForeignKey("EstudiosID")]
        public IEnumerable<Estudio> Estudios { get; set; }

        [Column("Idiomas")]
        [Display(Name = "Idiomas")]
        [ForeignKey("IdiomasID")]
        public IEnumerable<Lenguaje> Idiomas { get; set; }

        [Column("ConTecnicos")]
        [Display(Name = "Conocimientos Tecnicos")]
        [ForeignKey("ConAdicionalesID")]
        public IEnumerable<ConTecnicos> ConocimientosTecnicos { get; set; }

        [Column("ConAdicioneles")]
        [Display(Name = "Conocimientos Adicionales")]
        [ForeignKey("ConAdicionalesID")]
        public IEnumerable<ConAdicioneles> ConocimientosAdicionales { get; set; }
    }

    public class ConTecnicos
    {
        [Key]
        [Column("ConTecnicosID")]
        public Guid ConTecnicosID { get; set; }

        [Required]
        [StringLength(20)]
        [Column("Titulo")]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Conocimiento")]
        [Display(Name = "Conocimiento")]
        public string Conocimiento { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Nivel")]
        [Display(Name = "Nivel")]
        public string Nivel { get; set; }

        [ForeignKey("PersonaID")]
        public DatosPersonales datosPersonales { get; set; }
        [Required]
        public Guid PersonaID { get; set; }
    }

    public class ConAdicioneles
    {
        [Key]
        [Column("ConAdicionalesID")]
        public Guid ConAdicionalesID { get; set; }

        [Required]
        [StringLength(20)]
        [Column("Titulo")]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; }

        [Required]
        [StringLength(254)]
        [Column("Descripcion")]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [ForeignKey("PersonaID")]
        public DatosPersonales datosPersonales { get; set; }
        [Required]
        public Guid PersonaID { get; set; }
    }
    public class Lenguaje
    {
        [Key]
        [Column("IdiomasID")]
        public Guid IdiomasID { get; set; }

        [Required]
        [StringLength(20)]
        [Column("Idioma")]
        [Display(Name = "Idioma")]
        public string Idioma { get; set; }

        [Required]
        [StringLength(20)]
        [Column("NivelEscrito")]
        [Display(Name = "Nivel Escrito")]
        public string NivelEscrito { get; set; }

        [Required]
        [StringLength(20)]
        [Column("NivelLectura")]
        [Display(Name = "Nivel Lectura")]
        public string NivelLectura { get; set; }

        [Required]
        [StringLength(20)]
        [Column("NivelOral")]
        [Display(Name = "Nivel Oral")]
        public string NivelOral { get; set; }

        [ForeignKey("PersonaID")]
        public DatosPersonales datosPersonales { get; set; }
        [Required]
        public Guid PersonaID { get; set; }
    }


    [Table("Domicilio", Schema = "dbo")]
    public class Domicilio
    {
        [Key]
        [Column("DomicilioID")]
        public Guid DomicilioID { get; set; }

        [Required]
        [StringLength(20)]
        [Column("Pais")]
        [Display(Name = "Pais")]
        public string Pais { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Provincia")]
        [Display(Name = "Provincia")]
        public string Provincia { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Localidad")]
        [Display(Name = "Localidad")]
        public string Localidad { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Partido")]
        [Display(Name = "Partido")]
        public string Partido { get; set; }

        [Required]
        [Column("CodPostal")]
        [Display(Name = "Cod. Postal")]
        public int CodPostal { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Calle")]
        [Display(Name = "Calle")]
        public string Calle { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Numero")]
        [Display(Name = "Numero")]
        public int Numero { get; set; }

        [StringLength(50)]
        [Column("Piso")]
        [Display(Name = "Piso")]
        public string Piso { get; set; }

        [StringLength(50)]
        [Column("Depto")]
        [Display(Name = "Depto")]
        public string Depto { get; set; }

        [ForeignKey("PersonaID")]
        public DatosPersonales datosPersonales { get; set; }
        [Required]
        public Guid PersonaID { get; set; }
    }

    [Table("Experiencia", Schema = "dbo")]
    public class Experiencia
    {
        [Key]
        [Column("ExperienciaID")]
        public Guid ExperienciaID { get; set; }

        [Required]
        [Column("FechaDeInicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaDeInicio { get; set; }

        [Column("FechaDeFin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaDeFin { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Cargo")]
        public string Cargo { get; set; }

        [Required]
        [StringLength(50)]
        [Column("TipoDeEmpleo")]
        [Display(Name = "Tipo de empleo")]
        public string TipoDeEmpleo { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Empresa")]
        [Display(Name = "Empresa")]
        public string Empresa { get; set; }

        [StringLength(50)]
        [Column("Ubicacion")]
        [Display(Name = "Ubicación")]
        public string Ubicacion { get; set; }

        [StringLength(254)]
        [Column("Descripcion")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [ForeignKey("PersonaID")]
        public DatosPersonales datosPersonales { get; set; }
        [Required]
        public Guid PersonaID { get; set; }
    }

    [Table("Estudio", Schema = "dbo")]
    public class Estudio
    {
        [Key]
        [Column("EstudioID")]
        public Guid EstudioID { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Establecimiento")]
        [Display(Name = "Establecimiento")]
        public string Establecimiento { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Titulo")]
        [Display(Name = "Titulo")]
        public string Titulo { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Disciplina")]
        [Display(Name = "Disciplina académica")]
        public string Disciplina { get; set; }

        [Required]
        [Column("FechaDeInicio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de inicio")]
        public DateTime FechaDeInicio { get; set; }

        [Column("FechaDeFin")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de fin")]
        public DateTime FechaDeFin { get; set; }

        [Column("ActExtra")]
        [Display(Name = "Actividades extra curriculares")]
        public int ActExtra { get; set; }

        [Column("Descripcion")]
        [Display(Name = "Descripción")]
        public int Descripcion { get; set; }

        [ForeignKey("PersonaID")]
        public DatosPersonales datosPersonales { get; set; }
        [Required]
        public Guid PersonaID { get; set; }
    }
}
