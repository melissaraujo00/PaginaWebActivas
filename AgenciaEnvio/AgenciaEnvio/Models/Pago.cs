using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciaEnvio.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int ClienteId { get; set; }
        [ForeignKey("ClienteId")]
        public Usuario Cliente { get; set; }

        public int? EmpleadoId { get; set; }
        [ForeignKey("EmpleadoId")]
        public Usuario Empleado { get; set; }
        public int? SucursalId { get; set; }
        [ForeignKey("SucursalId")]
        public Sucursal Sucursal { get; set; }

        [Required]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public ICollection<DetallePago> DetallesPago { get; set; }
    }
}
