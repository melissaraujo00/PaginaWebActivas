using AgenciaEnvio.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgenciaEnvio.Models;

namespace AgenciaEnvio.Controllers
{
    public class SucursalController : Controller
    {
        private readonly AgenciaContex _context;

        public SucursalController(AgenciaContex context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var sucursales = await _context.Sucursales.ToListAsync();
            return View(sucursales);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sucursal sucursal)
        {
            ModelState.Remove("Usuarios");
            ModelState.Remove("Comisiones");
            ModelState.Remove("Pagos");
            ModelState.Remove("Envios");

            if (!ModelState.IsValid)
                return View(sucursal);

            _context.Sucursales.Add(sucursal);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal == null) return NotFound();

            return View(sucursal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sucursal sucursal)
        {
            if (id != sucursal.Id)
                return NotFound();

            ModelState.Remove("Usuarios");
            ModelState.Remove("Comisiones");
            ModelState.Remove("Pagos");
            ModelState.Remove("Envios");

            if (!ModelState.IsValid)
                return View(sucursal);

            try
            {
                _context.Update(sucursal);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Sucursales.Any(e => e.Id == sucursal.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var sucursal = await _context.Sucursales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sucursal == null) return NotFound();

            return View(sucursal);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal != null)
            {
                _context.Sucursales.Remove(sucursal);
                await _context.SaveChangesAsync();
            }
           
            return RedirectToAction(nameof(Index));
        }
    }
}

