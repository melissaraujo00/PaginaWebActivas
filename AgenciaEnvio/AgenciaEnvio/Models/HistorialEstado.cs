using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciaEnvio.Models
{
    public class HistorialEstado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EnvioId { get; set; }
        [ForeignKey("EnvioId")]
        public Envio Envio { get; set; }

        [Required]
        public int EstadoId { get; set; }
        [ForeignKey("EstadoId")]
        public EstadoEnvio EstadoEnvio { get; set; }

        [Required]
        public DateTime FechaActualizacion  { get; set; } = DateTime.Now;

        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }
        
        
        public int? SucursalId { get; set; }
        [ForeignKey("SucursalId")]

        public Sucursal? Sucursal { get; set; }

    }
}
