using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface ICancionesPresentacion
    {
        Task<List<Canciones>> Listar();
        Task<Canciones?> Guardar(Canciones? entidad);
        Task<Canciones?> Modificar(Canciones? entidad);
        Task<Canciones?> Borrar(Canciones? entidad);
    }
}