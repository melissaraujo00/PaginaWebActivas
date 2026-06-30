using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciaEnvio.Models
{
    public class Usuario
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }

        [StringLength(12)]
        [Required(ErrorMessage = "El número es obligatorio")]
        [RegularExpression(@"^[0-9\-]+$", ErrorMessage = "Solo se permiten números y guiones")]
        public string Telefono { get; set; }

        
        [StringLength(60)]
        public string? Email { get; set; }

        [Required]
        public int? SucursalId { get; set; }
        [ForeignKey("SucursalId")]

        public Sucursal? Sucursal { get; set; }

        [Required]
        [StringLength(30)]
        public string contrasena { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [Required]
        public bool EsAdmin { get; set; } = true;

        public DateTime? FechaEliminado { get; set; }

        public ICollection<Envio> Envios { get; set; }
        public ICollection<Auditoria> Auditorias { get; set; }
        public ICollection<HistorialEstado> HistorialEstados { get; set; }
        public ICollection<Pago> Pagos { get; set; }



    }
}
