using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciaEnvio.Models
{
    public class Destinatario
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string NombreCompleto { get; set; }

        [StringLength(15)]
        [Required(ErrorMessage = "El número es obligatorio")]
        [RegularExpression(@"^[0-9\-]+$", ErrorMessage = "Solo se permiten números y guiones")]
        public string Telefono { get; set; }

        [Required]
        public int CiudadId { get; set; }
        [ForeignKey("CiudadId")]

        public Ciudad Ciudad { get; set; }

        [Required]
        [StringLength(100)]
        public string ubicacion { get; set; }

        [Required]
        [StringLength(100)]
        public string NumeroCasa { get; set; }

        [Required]
        [StringLength(200)]
        public string Referencia { get; set; }

        public ICollection<Envio> Envios { get; set; }

    }
}
