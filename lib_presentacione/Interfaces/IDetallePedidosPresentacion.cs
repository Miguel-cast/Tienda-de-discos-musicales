using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IDetallePedidosPresentacion
    {
        Task<List<DetallePedidos>> Listar();
        Task<DetallePedidos?> Guardar(DetallePedidos? entidad);
        Task<DetallePedidos?> Modificar(DetallePedidos? entidad);
        Task<DetallePedidos?> Borrar(DetallePedidos? entidad);
    }
}