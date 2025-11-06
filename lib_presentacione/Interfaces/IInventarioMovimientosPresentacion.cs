using lib_dominio.Entidades;

namespace lib_presentaciones.Interfaces
{
    public interface IUsuariosSistemaPresentacion
    {
        Task<List<UsuariosSistema>> Listar();
        Task<UsuariosSistema?> Guardar(UsuariosSistema? entidad);
        Task<UsuariosSistema?> Modificar(UsuariosSistema? entidad);
        Task<UsuariosSistema?> Borrar(UsuariosSistema? entidad);
    }
}