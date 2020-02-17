using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace aspnet_core_api.Models
{
    [Table("Domicilio", Schema = "dbo")]
    public class Domicilio
    {
        [Key, Column("DomicilioID")]
        public Guid DomicilioID { get; set; }

        [Required, StringLength(20), Column("Pais"), Display(Name = "Pais"), DataType(DataType.Text)]
        public string Pais { get; set; }

        [Required, StringLength(50), Column("Provincia"), Display(Name = "Provincia"), DataType(DataType.Text)]
        public string Provincia { get; set; }

        [Required, StringLength(50), Column("Localidad"), Display(Name = "Localidad"), DataType(DataType.Text)]
        public string Localidad { get; set; }

        [Required, StringLength(50), Column("Partido"), Display(Name = "Partido"), DataType(DataType.Text)]
        public string Partido { get; set; }

        [Required, Column("CodPostal"), Display(Name = "Cod. Postal"), DataType(DataType.PostalCode)]
        public int CodPostal { get; set; }

        [Required, StringLength(50), Column("Calle"), Display(Name = "Calle"), DataType(DataType.Text)]
        public string Calle { get; set; }

        [Required, StringLength(50), Column("Numero"), Display(Name = "Numero"), DataType(DataType.PhoneNumber)]
        public int Numero { get; set; }

        [StringLength(50), Column("Piso"), Display(Name = "Piso"), DataType(DataType.Text)]
        public int Piso { get; set; }

        [StringLength(50), Column("Depto"), Display(Name = "Depto"), DataType(DataType.Text)]
        public string Depto { get; set; }




        [ForeignKey("Domicilios")]
        public Guid PersonaID { get; set; }
        public DatosPersonales Domicilios { get; set; }

    }
}