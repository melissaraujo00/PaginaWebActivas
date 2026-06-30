using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AgenciaEnvio.Models.Enums;

namespace AgenciaEnvio.Models
{
    public class Comision
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EnvioId { get; set; }
        [ForeignKey("EnvioId")]

        public Envio Envio { get; set; }

        [Required]
        public int SucursalId { get; set; }
        [ForeignKey("SucursalId")]
        public Sucursal Sucursal { get; set; }
        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal MontoBase { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; }

        [Required]
        public EstadoComision Estado { get; set; }

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
