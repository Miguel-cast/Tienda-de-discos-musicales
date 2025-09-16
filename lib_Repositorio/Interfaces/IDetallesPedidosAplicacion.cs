using lib_dominio.Entidades;
using System.Collections.Generic;

namespace lib_repositorios.Interfaces
{
    public interface IDetallesPedidosAplicacion
    {
        void Configurar(string StringConexion);
        List<DetallesPedidos> Listar();
        DetallesPedidos? Guardar(DetallesPedidos? entidad);
        DetallesPedidos? Modificar(DetallesPedidos? entidad);
        DetallesPedidos? Borrar(DetallesPedidos? entidad);
    }
}