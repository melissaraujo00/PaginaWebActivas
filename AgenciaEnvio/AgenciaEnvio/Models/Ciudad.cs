using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgenciaEnvio.Models
{
    public class Ciudad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        public int DepartamentoId { get; set; }
        [ForeignKey("DepartamentoId")]

        public Departamento Departamento { get; set; }



    }
}
