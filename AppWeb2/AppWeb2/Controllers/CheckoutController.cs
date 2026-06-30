using AppWeb2.Data;
using AppWeb2.Filtros;
using AppWeb2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace AppWeb2.Controllers
{
    [SessionAuthorize]
    public class CheckoutController : Controller
    {
        private readonly TiendaContext _context;
        private readonly string PaypalClientId;
        private readonly string PaypalSecret;
        private readonly string PaypalUrl;

        public CheckoutController(IConfiguration configuration, TiendaContext context)
        {
            _context = context;
            PaypalClientId = configuration["PayPalSettings:ClientId"]!;
            PaypalSecret = configuration["PayPalSettings:Secret"]!;
            PaypalUrl = configuration["PayPalSettings:Url"]!;
        }

        public IActionResult Index()
        {
            ViewBag.PaypalClientId = PaypalClientId;
            return View();
        }

        public IActionResult Buy(int id, decimal precioFinal, string titulo)
        {
            HttpContext.Session.SetInt32("JuegoId", id);
            HttpContext.Session.SetString("Titulo", titulo);
            HttpContext.Session.SetString("PrecioFinal", precioFinal.ToString(CultureInfo.InvariantCulture));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<JsonResult> CreateOrder([FromBody] JsonObject data)
        {
            var totalAmount = data?["amount"]?.ToString();
            if (string.IsNullOrEmpty(totalAmount))
                return new JsonResult(new { Id = "" });

            var orderRequest = new
            {
                intent = "CAPTURE",
                purchase_units = new[]
                {
                    new
                    {
                        amount = new
                        {
                            currency_code = "USD",
                            value = totalAmount
                        }
                    }
                }
            };

            string accessToken = await GetPaypalAccessToken();
            if (string.IsNullOrEmpty(accessToken))
                return new JsonResult(new { Id = "" });

            string url = $"{PaypalUrl}/v2/checkout/orders";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var jsonRequest = JsonSerializer.Serialize(orderRequest);
                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var order = JsonNode.Parse(responseBody);
                    string paypalOrderId = order?["id"]?.ToString() ?? "";
                    return new JsonResult(new { Id = paypalOrderId });
                }
                return new JsonResult(new { Id = "" });
            }
        }

        [HttpPost]
        public async Task<JsonResult> CompleteOrder([FromBody] JsonObject data)
        {
            var orderId = data?["orderID"]?.ToString();
            var juegoId = data?["juegoId"]?.GetValue<int>();
            var precioFinal = data?["precioFinal"]?.GetValue<decimal>();

            if (string.IsNullOrEmpty(orderId) || !juegoId.HasValue || !precioFinal.HasValue)
                return new JsonResult(new { success = false, error = "Datos incompletos" });

            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
                return new JsonResult(new { success = false, error = "Usuario no autenticado" });

            string accessToken = await GetPaypalAccessToken();
            if (string.IsNullOrEmpty(accessToken))
                return new JsonResult(new { success = false, error = "No se pudo obtener token de PayPal" });

            string url = $"{PaypalUrl}/v2/checkout/orders/{orderId}/capture";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent("", Encoding.UTF8, "application/json");
                var response = await client.PostAsync(url, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return new JsonResult(new { success = false, statusCode = (int)response.StatusCode, error = responseBody });

                var jsonResponse = JsonNode.Parse(responseBody);
                if (jsonResponse?["status"]?.ToString() != "COMPLETED")
                    return new JsonResult(new { success = false, error = "PayPal no completó la captura", details = responseBody });

                var compra = new Compra
                {
                    UsuarioId = usuarioId.Value,
                    FechaCompra = DateTime.Now
                };
                _context.Compras.Add(compra);
                await _context.SaveChangesAsync();

                var detalle = new DetalleCompra
                {
                    idCompra = compra.Id,
                    VideoJuegoId = juegoId.Value,
                    cantidad = 1,
                    total = precioFinal.Value,
                    estadoCompra = "Completado",
                    fechaHoraTransaccion = DateTime.Now,
                    codigoTransaccion = orderId
                };
                _context.DetalleCompras.Add(detalle);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("JuegoId");
                HttpContext.Session.Remove("Titulo");
                HttpContext.Session.Remove("PrecioFinal");

                return new JsonResult(new { success = true });
            }
        }

        private async Task<string> GetPaypalAccessToken()
        {
            string url = $"{PaypalUrl}/v1/oauth2/token";
            using (var client = new HttpClient())
            {
                string credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{PaypalClientId}:{PaypalSecret}"));
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var data = JsonNode.Parse(responseBody);
                    return data?["access_token"]?.ToString() ?? "";
                }
                return "";
            }
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}