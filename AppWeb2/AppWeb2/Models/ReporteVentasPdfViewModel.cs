namespace AppWeb2.Models
{
    public class ReporteVentasPdfViewModel
    {
        public FiltrosPaginacionViewModel Filtros { get; set; }
        public List<VentaViewModel> Ventas { get; set; }

        public string NombreUsuario { get; set; }
        public string NombreEmpresa { get; set; } = "GameStore S.A. de C.V.";
        public string TituloReporte { get; set; } = "HISTORIAL DE COMPRAS";
        public string DescripcionEmpresa { get; set; } = "Especialistas en software y videojuegos.";
        public string CorreoEmpresa { get; set; } = "contacto@gamestore.com";
        public string DireccionEmpresa { get; set; } = "San Miguel, El Salvador";
    }
}
