using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IPedidosAplicacion
    {
        void Configurar(string StringConexion);
        List<Pedidos> Listar();
        Pedidos? Guardar(Pedidos? entidad);
        Pedidos? Modificar(Pedidos? entidad);
        Pedidos? Borrar(Pedidos? entidad);
        List<Pedidos> ObtenerPedidosPorCliente(int clienteId);
        List<Pedidos> ObtenerPedidosPorEstado(string estado);
        int ContarPedidosPorMes(int año, int mes);
        List<Pedidos> ObtenerPedidosRecientes(int dias = 7);
    }
}