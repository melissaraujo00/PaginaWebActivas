using Aplication.DTOs;
using Domian.Entities;
using Domian.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplication.Services
{
    public class UsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<List<Usuario>> ObtenerTodos()
        {
            return await _usuarioRepository.ObtenerTodos();
        }

        public async Task<Usuario> CrearUsuario(UsuarioDTO dto)
        {
            Usuario usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Corre = dto.Corre
            };
            return await _usuarioRepository.CrearUsuario(usuario);

            
        }
    }
}
