using clase31.Models;

namespace clase31.Services
{
    public interface ILibroService
    {
        Task<List<Libro>> ObtenerTodos();
        Task<Libro?> ObtenerPorId(int id);
        Task<Libro> Crear(Libro libro);
        Task<Libro?> Actualizar(int id, Libro libro);
        Task<bool> Eliminar(int id);
    }
}
