using System.ComponentModel.DataAnnotations;

namespace AppWeb2.Models
{
    public class VentaViewModel
    {
        public int idCompra { get; set; }

        public DateTime FechaCompra { get; set; } = DateTime.Now;
        public int UsuarioId { get; set; }

        public string clienteNombre { get; set; }
        public int VideoJuegoId { get; set; }
        public string TituloJuego { get; set; }

        public int cantidad { get; set; }

        public decimal total { get; set; }

        public string estadoCompra { get; set; }

        public DateTime fechaHoraTransaccion { get; set; }

        public string codigoTransaccion { get; set; }

    }
}
