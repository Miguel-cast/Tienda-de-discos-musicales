using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public partial class Conexion : DbContext, IConexion
    {
        public string? StringConexion { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.StringConexion!, p => { });
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        
        public DbSet<Artistas>? Artistas { get; set; }
        public DbSet<Canciones>? Canciones { get; set; }
        public DbSet<Clientes>? Clientes { get; set; }
        public DbSet<DetallePedidos>? DetallePedidos { get; set; }
        public DbSet<Discos>? Discos { get; set; }
        public DbSet<Empleados>? Empleados { get; set; }
        public DbSet<Envios>? Envios { get; set; }
        public DbSet<Facturas>? Facturas { get; set; }
        public DbSet<Generos>? Generos { get; set; }
        public DbSet<InventarioMovimientos>? InventarioMovimientos { get; set; }
        public DbSet<Pagos>? Pagos { get; set; }
        public DbSet<Pedidos>? Pedidos { get; set; }
        public DbSet<Proveedores>? Proveedores { get; set; }
        public DbSet<ReseñasClientes>? ReseñasClientes { get; set; }
        public DbSet<UsuariosSistema>? UsuariosSistema { get; set; }

    }
}