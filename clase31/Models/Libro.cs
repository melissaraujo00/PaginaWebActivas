using System.ComponentModel.DataAnnotations;

namespace clase31.Models
{
    public class Libro
    {
        [Key]
        public int Id { get; set; }  

        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [StringLength(200)]
        public string Autor { get; set; }

        public int AnioPublicacion { get; set; }

        public string Genero { get; set; }

        [Range(0, 10000)]
        public int Paginas { get; set; }

        public bool Disponible { get; set; } = true;  
    }
}
