using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciaEnvio.Models
{
    public class DetallePago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PagoId { get; set; }
        [ForeignKey("PagoId")]

        public Pago Pago { get; set; }
        [Required]
        public int EnviosId { get; set; }
        [ForeignKey("EnviosId")]
        public Envio Envio { get; set; }

        [StringLength(200)]
        public string? Detalles { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Subtotal { get; set; }
    }
}
