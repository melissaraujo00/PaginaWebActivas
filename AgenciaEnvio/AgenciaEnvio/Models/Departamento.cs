using System.ComponentModel.DataAnnotations;

namespace AgenciaEnvio.Models
{
    public class Departamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        public ICollection<Ciudad> ciudades { get; set; }
    }
}
