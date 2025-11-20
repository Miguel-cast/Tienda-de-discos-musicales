using lib_dominio.Entidades;
using System.Diagnostics.Metrics;

namespace lib_presentaciones.Interfaces
{
    public interface ICancionesPresentacion
    {
        Task<List<Canciones>> Listar();
        Task<Canciones?> Guardar(Canciones? entidad);
        Task<List<Canciones>> PorTitulo(Canciones? entidad);
        Task<Canciones?> Modificar(Canciones? entidad);
        Task<Canciones?> Borrar(Canciones? entidad);
    }
}