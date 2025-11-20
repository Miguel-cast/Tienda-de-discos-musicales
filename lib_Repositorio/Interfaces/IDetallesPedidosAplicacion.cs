using lib_dominio.Entidades;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace lib_repositorios.Interfaces
{
    public interface IDetallesPedidosAplicacion
    {
        void Configurar(string StringConexion);
        List<DetallePedidos> Listar();
        DetallePedidos? Guardar(DetallePedidos? entidad);
        DetallePedidos? Modificar(DetallePedidos? entidad);
        DetallePedidos? Borrar(DetallePedidos? entidad);
    }
}