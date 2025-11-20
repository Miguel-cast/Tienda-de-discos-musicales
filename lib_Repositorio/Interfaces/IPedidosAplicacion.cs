using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IPedidosAplicacion
    {
        void Configurar(string StringConexion);
        List<Pedidos> Listar();
        List<Pedidos> PorEstado(Pedidos? entidad);
        Pedidos? Guardar(Pedidos? entidad);
        Pedidos? Modificar(Pedidos? entidad);
        Pedidos? Borrar(Pedidos? entidad);
    }
}