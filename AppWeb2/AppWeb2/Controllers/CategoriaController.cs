using AppWeb2.Data;
using AppWeb2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AppWeb2.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly TiendaContext _context;

        public CategoriaController(TiendaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _context.Categorias
                .Include(c => c.VideoJuegos)
                .ToListAsync();
            return View(categorias);
        }

        public async Task<IActionResult> JuegosPorCategoria(int id)
        {
            var categoria = await _context.Categorias
                .Include(c => c.VideoJuegos)
                .FirstOrDefaultAsync(c => c.id == id);

            if (categoria == null)
                return NotFound();

            ViewBag.CategoriaNombre = categoria.Nombre;
            var juegos = categoria.VideoJuegos.ToList();

            return View(juegos);
        }
    }
}