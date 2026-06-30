using Aplication.DTOs;
using Aplication.Services;
using Microsoft.AspNetCore.Mvc;

namespace clase33.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _service;
        
        public UsuarioController(UsuarioService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Obtener() 
        { 
           var usuarios = await _service.ObtenerTodos();
           return Ok(usuarios);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(UsuarioDTO dto)
        {
            var usuarioCreado = await _service.CrearUsuario(dto);

            return Ok(usuarioCreado);
        }
    }
}
