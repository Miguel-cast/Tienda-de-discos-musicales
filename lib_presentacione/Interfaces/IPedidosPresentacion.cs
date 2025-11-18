using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IPedidosPresentacion
    {
        Task<List<Pedidos>> Listar();
        Task<Pedidos?> Guardar(Pedidos? entidad);
        Task<Pedidos?> Modificar(Pedidos? entidad);
        Task<Pedidos?> Borrar(Pedidos? entidad);
    }
}