using clase31.Models;
using clase31.Services;
using Microsoft.AspNetCore.Mvc;

namespace clase31.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibrosController : Controller
    {
            private readonly ILibroService _service;

            public LibrosController(ILibroService service)
            {
                _service = service;
            }

            [HttpGet]
            public async Task<IActionResult> ObtenerTodos()
            {
                var libros = await _service.ObtenerTodos();
                return Ok(libros);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> ObtenerPorId(int id)
            {
                var libro = await _service.ObtenerPorId(id);
                if (libro == null) return NotFound();
                return Ok(libro);
            }

            [HttpPost]
            public async Task<IActionResult> Crear([FromBody] Libro libro)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var nuevo = await _service.Crear(libro);
                return CreatedAtAction(nameof(ObtenerPorId), new { id = nuevo.Id }, nuevo);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Actualizar(int id, [FromBody] Libro libro)
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);
                var actualizado = await _service.Actualizar(id, libro);
                if (actualizado == null) return NotFound();
                return Ok(actualizado);
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Eliminar(int id)
            {
                var eliminado = await _service.Eliminar(id);
                if (!eliminado) return NotFound();
                return Ok("Libro eliminado");
            }
        }
    
}
