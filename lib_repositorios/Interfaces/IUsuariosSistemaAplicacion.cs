using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IUsuariosSistemaAplicacion
    {
        void Configurar(string StringConexion);
        UsuariosSistema? Guardar(UsuariosSistema? entidad);
        UsuariosSistema? Modificar(UsuariosSistema? entidad);
        UsuariosSistema? Borrar(UsuariosSistema? entidad);
        List<UsuariosSistema> Listar();
    }
}