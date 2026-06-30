using clase31.Models;

namespace clase31.Repositories
{
    public interface ILibroRepository
    {
        Task<List<Libro>> ObtenerTodos();
        Task<Libro?> ObtenerPorId(int id);
        Task<Libro> Crear(Libro libro);
        Task<Libro?> Actualizar(Libro libro);
        Task<bool> Eliminar(int id);

    }
}
