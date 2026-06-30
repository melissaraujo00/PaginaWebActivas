using System.ComponentModel.DataAnnotations;

namespace AgenciaEnvio.Models
{
    public class Sucursal
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }

        
        [StringLength(12)]
        [Required(ErrorMessage = "El número es obligatorio")]
        [RegularExpression(@"^[0-9\-]+$", ErrorMessage = "Solo se permiten números y guiones")]
        public string Telefono { get; set; }

        public ICollection<Usuario> Usuarios { get; set; }

        public ICollection<Comision> Comisiones { get; set; }
        public ICollection<Pago> Pagos { get; set; }
        public ICollection<Envio> Envios { get; set; }
    }
}
