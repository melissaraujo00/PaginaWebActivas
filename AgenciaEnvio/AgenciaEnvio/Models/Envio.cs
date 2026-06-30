using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciaEnvio.Models
{
    public class Envio
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(25)]
        public string CodigoRastreo { get; set; }

        [Required]
        public int EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public EstadoEnvio EstadoEnvio { get; set; }

        [Required]
        public int RemitenteId { get; set; }
        [ForeignKey("RemitenteId")]
        public Usuario Remitente { get; set; }
        
        [Required]
        public int DestinatarioId { get; set; }
        [ForeignKey("DestinatarioId")]
        public Destinatario Destinatario { get; set; }

        public int? SucursalId { get; set; }

        [ForeignKey("SucursalId")]
        public Sucursal Sucursal { get; set; }

        [Required]
        public DateTime FechaEnvio { get; set; } = DateTime.Now;
        [Required]
        public DateTime FechaEntrega { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Costo { get; set; }

        public ICollection<Paquete> Paquetes { get; set; }
        public ICollection<HistorialEstado> HistorialEstados { get; set; }

        public ICollection<Comision> Comisiones { get; set; }
        public ICollection<DetallePago> DetallesPago { get; set; }

    }
}
