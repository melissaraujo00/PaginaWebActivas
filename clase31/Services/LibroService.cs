using clase31.Models;
using clase31.Repositories;

namespace clase31.Services
{
    public class LibroService : ILibroService
    {
        private readonly ILibroRepository _repository;

        public LibroService(ILibroRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Libro>> ObtenerTodos()
        {
            return await _repository.ObtenerTodos();
        }

        public async Task<Libro?> ObtenerPorId(int id)
        {
            return await _repository.ObtenerPorId(id);
        }

        public async Task<Libro> Crear(Libro libro)
        {
            return await _repository.Crear(libro);
        }

        public async Task<Libro?> Actualizar(int id, Libro libro)
        {
            var existente = await _repository.ObtenerPorId(id);
            if (existente == null) return null;

            // Actualizar propiedades
            existente.Titulo = libro.Titulo;
            existente.Autor = libro.Autor;
            existente.AnioPublicacion = libro.AnioPublicacion;
            existente.Genero = libro.Genero;
            existente.Paginas = libro.Paginas;
            existente.Disponible = libro.Disponible;

            return await _repository.Actualizar(existente);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _repository.Eliminar(id);
        }
    }
}
