using lib_dominio.Entidades;
using System.Diagnostics.Metrics;

namespace lib_presentaciones.Interfaces
{
    public interface IGenerosPresentacion
    {
        Task<List<Generos>> Listar();
        Task<List<Generos>> PorNombreGenero(Generos? entidad);
        Task<Generos?> Guardar(Generos? entidad);
        Task<Generos?> Modificar(Generos? entidad);
        Task<Generos?> Borrar(Generos? entidad);
    }
}