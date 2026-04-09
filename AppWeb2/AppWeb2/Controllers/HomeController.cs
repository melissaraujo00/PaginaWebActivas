using AppWeb2.Data;
using AppWeb2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace AppWeb2.Controllers
{
    public class HomeController : Controller
    {
        private readonly TiendaContext _context;
        public HomeController(TiendaContext context) 
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var videojuegos = await _context.VideoJuegos.ToListAsync();
            return View(videojuegos);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contactos()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Calcular(Notas model)
        {
            model.promedio = (model.n1 + model.n2 + model.n3) / 3;
            Console.WriteLine($"Promedio calculado: {model.promedio}");

            return View("Contactos", model);
        }
    }
}
