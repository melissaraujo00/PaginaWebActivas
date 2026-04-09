using AppWeb2.Data;
using AppWeb2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AppWeb2.Controllers
{
    public class VideoJuegosController : Controller
    {
        private readonly TiendaContext _context;

        public VideoJuegosController(TiendaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var juegos = await _context.VideoJuegos.Include(j => j.Categoria).ToListAsync();
            return View(juegos);
        }

        public IActionResult Create()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias.ToList(), "id", "Nombre");
            return View(new VideoJuego());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VideoJuego juego, IFormFile archivoImagen)
        {
            if (ModelState.IsValid)
            {
                if (archivoImagen != null && archivoImagen.Length > 0)
                {
                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivoImagen.FileName);
                    var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);

                    using (var stream = new FileStream(ruta, FileMode.Create))
                    {
                        await archivoImagen.CopyToAsync(stream);
                    }

                    juego.imagen = "/imagenes/" + nombreArchivo;
                }

                _context.VideoJuegos.Add(juego);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = new SelectList(await _context.Categorias.ToListAsync(), "id", "Nombre", juego.CategoriaId);
            return View(juego);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var juego = await _context.VideoJuegos
                .Include(j => j.Categoria)
                .FirstOrDefaultAsync(j => j.Id == id);
            if (juego == null) return NotFound();

            ViewBag.Categorias = new SelectList(await _context.Categorias.ToListAsync(), "id", "Nombre", juego.CategoriaId);
            return View(juego);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VideoJuego juego, IFormFile? archivoImagen)
        {
            if (id != juego.Id)
                return NotFound();

            var juegoDB = await _context.VideoJuegos.FindAsync(id);
            if (juegoDB == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                juegoDB.Titulo = juego.Titulo;
                juegoDB.Precio = juego.Precio;
                juegoDB.CategoriaId = juego.CategoriaId;
                juegoDB.Descripcion = juego.Descripcion;
                juegoDB.EdadMinima = juego.EdadMinima;
                juegoDB.EnDescuento = juego.EnDescuento;
                juegoDB.PrecioDescuento = juego.PrecioDescuento;
                juegoDB.FechaInicioPromo = juego.FechaInicioPromo;
                juegoDB.FechaFinPromo = juego.FechaFinPromo;

                if (archivoImagen != null && archivoImagen.Length > 0)
                {
                    if (!string.IsNullOrEmpty(juegoDB.imagen))
                    {
                        var rutaAnterior = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", juegoDB.imagen.TrimStart('/'));
                        if (System.IO.File.Exists(rutaAnterior))
                            System.IO.File.Delete(rutaAnterior);
                    }

                    var nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(archivoImagen.FileName);
                    var rutaNueva = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagenes", nombreArchivo);
                    using (var stream = new FileStream(rutaNueva, FileMode.Create))
                    {
                        await archivoImagen.CopyToAsync(stream);
                    }
                    juegoDB.imagen = "/imagenes/" + nombreArchivo;
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categorias = new SelectList(await _context.Categorias.ToListAsync(), "id", "Nombre", juego.CategoriaId);
            return View(juego);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var juego = await _context.VideoJuegos
                .Include(j => j.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (juego == null) return NotFound();
            return View(juego);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var juego = await _context.VideoJuegos.FindAsync(id);
            if (juego != null)
            {
                _context.VideoJuegos.Remove(juego);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Promocion()
        {
            var hoy = DateTime.Today;
            var juegos = await _context.VideoJuegos
                .Include(j => j.Categoria)
                .Where(j => j.EnDescuento
                            && j.FechaInicioPromo.HasValue
                            && j.FechaFinPromo.HasValue
                            && j.FechaInicioPromo.Value.Date <= hoy
                            && j.FechaFinPromo.Value.Date >= hoy)
                .ToListAsync();

            return View(juegos);
        }

        public async Task<IActionResult> Nuevo(int cantidad = 15)
        {
            var juegos = await _context.VideoJuegos
                .Include(j => j.Categoria)
                .OrderByDescending(j => j.FechaRegistro)
                .Take(cantidad)
                .ToListAsync();

            return View(juegos);
        }
    }
}