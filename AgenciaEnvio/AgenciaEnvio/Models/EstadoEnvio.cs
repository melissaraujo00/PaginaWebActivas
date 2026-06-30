using System.ComponentModel.DataAnnotations;

namespace AgenciaEnvio.Models
{
    public class EstadoEnvio
    {
        [Key]
        public int EstadoId { get; set; }

        [Required]
        [StringLength(50)]
        public string NombreEstado { get; set; }

        [Required]
        [StringLength(100)]
        public string Descripcion { get; set; }

        public ICollection<HistorialEstado> Historiales { get; set; }
    }
}
