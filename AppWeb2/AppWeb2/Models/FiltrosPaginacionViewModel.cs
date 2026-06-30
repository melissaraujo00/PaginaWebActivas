namespace AppWeb2.Models
{
    public class FiltrosPaginacionViewModel
    {
        public string Desde { get; set; }
        public string Hasta { get; set; }

        public string SearchString { get; set; }

        public string LabelSearch { get; set; } = "BUSCAR";

        public string Estado { get; set; }

        public bool MostrarEstado { get; set; }

        public int PaginaActual { get; set; }
        public int TotalPaginas { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
    }
}
