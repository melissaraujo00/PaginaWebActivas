using AppWeb2.Models;
using Microsoft.EntityFrameworkCore;

namespace AppWeb2.Data
{
    public class TiendaContext : DbContext
    {
        public TiendaContext(DbContextOptions<TiendaContext> options) 
            : base(options)
        {
        }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<VideoJuego> VideoJuegos { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)

                .IsUnique();
            modelBuilder.Entity<VideoJuego>()
                .HasOne(v => v.Categoria)
                .WithMany(c => c.VideoJuegos)
                .HasForeignKey(v => v.CategoriaId);




        }
        
    }
}
