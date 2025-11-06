using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IDiscosPresentacion
    {
        Task<List<Discos>> Listar();
        Task<Discos?> Guardar(Discos? entidad);
        Task<Discos?> Modificar(Discos? entidad);
        Task<Discos?> Borrar(Discos? entidad);
    }
}