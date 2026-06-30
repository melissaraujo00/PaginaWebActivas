using System.ComponentModel.DataAnnotations;

namespace AppWeb2.Models
{
    public class Rol
    {
        [Key]
        public int idRol { get; set; }

        public string rol { get; set; }

        

    }
}
