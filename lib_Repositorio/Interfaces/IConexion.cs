using lib_dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace lib_repositorios.Interfaces
{
    public interface IConexion
    {
        string? StringConexion { get; set; }
        DbSet<Artistas>? Artistas { get; set; }
        DbSet<Canciones>? Canciones { get; set; }
        DbSet<Clientes>? Clientes { get; set; }
        DbSet<DetallePedidos>? DetallePedidos { get; set; }
        DbSet<Discos>? Discos { get; set; }
        DbSet<Empleados>? Empleados { get; set; }
        DbSet<Envios>? Envios { get; set; }
        DbSet<Facturas>? Facturas { get; set; }
        DbSet<Generos>? Generos { get; set; }
        DbSet<InventarioMovimientos>? InventarioMovimientos { get; set; }
        DbSet<Pagos>? Pagos { get; set; }
        DbSet<Pedidos>? Pedidos { get; set; }
        DbSet<Proveedores>? Proveedores { get; set; }
        DbSet<ReseñasClientes>? ReseñasClientes { get; set; }
        DbSet<UsuariosSistema>? UsuariosSistema { get; set; }

        EntityEntry<T> Entry<T>(T entity) where T : class;
        int SaveChanges();
    }
}