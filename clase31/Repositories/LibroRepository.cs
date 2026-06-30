using clase31.Data;
using clase31.Models;
using Microsoft.EntityFrameworkCore;

namespace clase31.Repositories
{
    public class LibroRepository: ILibroRepository
    {


            private readonly ApiContext _context;

            public LibroRepository(ApiContext context)
            {
                _context = context;
            }

            public async Task<List<Libro>> ObtenerTodos()
            {
                return await _context.Libros.ToListAsync();
            }

            public async Task<Libro?> ObtenerPorId(int id)
            {
                return await _context.Libros.FindAsync(id);
            }

            public async Task<Libro> Crear(Libro libro)
            {
                _context.Libros.Add(libro);
                await _context.SaveChangesAsync();
                return libro;
            }

            public async Task<Libro?> Actualizar(Libro libro)
            {
                var existente = await _context.Libros.FindAsync(libro.Id);
                if (existente == null) return null;

                _context.Entry(existente).CurrentValues.SetValues(libro);
                await _context.SaveChangesAsync();
                return existente;
            }

            public async Task<bool> Eliminar(int id)
            {
                var libro = await _context.Libros.FindAsync(id);
                if (libro == null) return false;

                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
                return true;
            }
        }
    
}
