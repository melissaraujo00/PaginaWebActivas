using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciaEnvio.Models
{
    public class Paquete
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EnvioId { get; set; }
        [ForeignKey("EnvioId")]
        public Envio Envio { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Peso { get; set; }

        [Required]
        [StringLength(200)]
        public string Contenido { get; set; }
    }
}
