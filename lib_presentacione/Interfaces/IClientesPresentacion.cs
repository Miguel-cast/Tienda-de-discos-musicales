using lib_dominio.Entidades;
using System.Diagnostics.Metrics;

namespace lib_presentaciones.Interfaces
{
    public interface IClientesPresentacion
    {
        Task<List<Clientes>> Listar();
        Task<List<Clientes>> PorNombre(Clientes? entidad);
        Task<Clientes?> Guardar(Clientes? entidad);
        Task<Clientes?> Modificar(Clientes? entidad);
        Task<Clientes?> Borrar(Clientes? entidad);
    }
}