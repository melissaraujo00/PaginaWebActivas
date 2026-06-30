namespace AppWeb2.Helpers
{
    public static class FileNamingHelper
    {

        public static string GenerarNombrePdf(string baseName, string search = null, string estado = null, DateTime? desde = null, DateTime? hasta = null)
        {

            var partes = new List<string> { baseName };

            if (!string.IsNullOrWhiteSpace(search))
            {
                partes.Add($"Filtro_{search.Replace(" ", "_")}");
            }

            if (!string.IsNullOrWhiteSpace(estado))
            {
                partes.Add(estado);
            }

            if (desde.HasValue || hasta.HasValue)
            {
                partes.Add(DateTime.Now.ToString("yyyyMMdd"));
            }

            return $"{string.Join("_", partes)}.pdf";
        }
    }
}