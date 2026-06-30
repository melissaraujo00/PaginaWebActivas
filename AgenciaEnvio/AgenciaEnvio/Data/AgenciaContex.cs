using AgenciaEnvio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AgenciaEnvio.Data
{
    public class AgenciaContex : DbContext
    {
        public AgenciaContex(DbContextOptions<AgenciaContex> options) : base(options)
        {
        }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Ciudad> Ciudades { get; set; }
        public DbSet<Usuario> Clientes { get; set; }
        public DbSet<Destinatario> Destinatarios { get; set; }
        public DbSet<EstadoEnvio> EstadoEnvios { get; set; }
        public DbSet<Envio> Envios { get; set; }
        public DbSet<Paquete> Paquetes { get; set; }

        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<EstadoEnvio> EstadosEnvio { get; set; }
        public DbSet<HistorialEstado> HistorialEstados { get; set; }
        public DbSet<Comision> Comisiones { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<DetallePago> DetallesPago { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Envio>()
                .HasOne(e => e.Remitente) 
                .WithMany()
                .HasForeignKey(e => e.RemitenteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Envio>()
                .HasOne(e => e.Destinatario)
                .WithMany()
                .HasForeignKey(e => e.DestinatarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Envio>()
                .HasOne(e => e.Sucursal)
                .WithMany()
                .HasForeignKey(e => e.SucursalId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Empleado)
                .WithMany()
                .HasForeignKey(p => p.EmpleadoId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Envio>()
                .HasOne(e => e.EstadoEnvio) 
                .WithMany()
                .HasForeignKey(e => e.EstadoId)
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
