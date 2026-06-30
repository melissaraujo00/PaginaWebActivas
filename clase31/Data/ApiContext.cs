using Microsoft.EntityFrameworkCore;

namespace clase31.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }
        public DbSet<Models.Libro> Libros { get; set; }
    }
}
