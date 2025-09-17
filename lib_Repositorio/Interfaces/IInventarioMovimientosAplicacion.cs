using lib_dominio.Entidades;
using System.Collections.Generic;

namespace lib_repositorios.Interfaces
{
    public interface IInventarioMovimientosAplicacion
    {
        void Configurar(string StringConexion);
        List<InventarioMovimientos> Listar();
        InventarioMovimientos? Guardar(InventarioMovimientos? entidad);
        InventarioMovimientos? Modificar(InventarioMovimientos? entidad);
        InventarioMovimientos? Borrar(InventarioMovimientos? entidad);
    }
}