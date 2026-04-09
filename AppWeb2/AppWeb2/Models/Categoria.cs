using System.ComponentModel.DataAnnotations;

namespace AppWeb2.Models
{
    public class Categoria
    {
        [Key]
        public int id { get; set; }
        [Required]
        [StringLength(75)]
        public string Nombre { get; set; }
        [StringLength(255)]
        public string? Descripcion { get; set; }

        public List<VideoJuego>? VideoJuegos { get; set; }
        
    }
}
