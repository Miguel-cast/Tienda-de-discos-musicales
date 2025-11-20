using lib_dominio.Entidades;
using System.Diagnostics.Metrics;

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