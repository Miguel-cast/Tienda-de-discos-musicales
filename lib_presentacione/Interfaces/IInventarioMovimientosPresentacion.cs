using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IInventarioMovimientosPresentacion
    {
        Task<List<InventarioMovimientos>> Listar();
        Task<InventarioMovimientos?> Guardar(InventarioMovimientos? entidad);
        Task<InventarioMovimientos?> Modificar(InventarioMovimientos? entidad);
        Task<InventarioMovimientos?> Borrar(InventarioMovimientos? entidad);
    }
}