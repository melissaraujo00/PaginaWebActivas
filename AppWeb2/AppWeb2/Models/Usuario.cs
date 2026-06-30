using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWeb2.Models
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public Byte[] Password { get; set; }

        [StringLength(50)]
        public string Salt { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        // Foreign key property
        public int idRol { get; set; }

        // Navigation property with ForeignKey attribute
        [ForeignKey("idRol")]
        public Rol Rol { get; set; }




    }
}
