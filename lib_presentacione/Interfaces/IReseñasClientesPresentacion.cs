using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IReseñasClientesPresentacion
    {
        Task<List<ReseñasClientes>> Listar();
        Task<ReseñasClientes?> Guardar(ReseñasClientes? entidad);
        Task<ReseñasClientes?> Modificar(ReseñasClientes? entidad);
        Task<ReseñasClientes?> Borrar(ReseñasClientes? entidad);
    }
}