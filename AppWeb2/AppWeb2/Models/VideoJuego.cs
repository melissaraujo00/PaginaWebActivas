using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppWeb2.Models
{
    public class VideoJuego
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        [Required]
        public int CategoriaId { get; set; }
        public Categoria? Categoria { get; set; }
        [ForeignKey("CategoriaId")]

        [StringLength(500)]
        public string Descripcion { get; set; }

        public string? imagen { get; set; }
        [Required]
        public  string EdadMinima { get; set; }
        [Required]
        public Boolean EnDescuento { get; set; } = false;

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PrecioDescuento { get; set; }
        public DateTime? FechaInicioPromo { get; set; }
        public DateTime? FechaFinPromo { get; set; }

        public DateTime FechaRegistro { get; set; } = DateTime.Now;


        //public ICollection<Compra> Compras { get; set; }
    }
}
