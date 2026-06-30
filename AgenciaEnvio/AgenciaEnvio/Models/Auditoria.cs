using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AgenciaEnvio.Models.Enums;

namespace AgenciaEnvio.Models
{
    public class Auditoria
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]

        public Usuario Usuario { get; set; }

        [Required]
        public string TablaAfectada { get; set; }

        [Required]
        public AccionAuditoria AccionAuditoria { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string ValoresAntiguos { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string ValoresNuevos { get; set; }

        [Required]
        public DateTime FechaCambio { get; set; }


    }
}
    