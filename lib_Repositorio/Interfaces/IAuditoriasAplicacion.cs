using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IAuditoriasAplicacion
    {
        void Configurar(string StringConexion);
        List<Auditorias> Listar();
        Auditorias? Guardar(Auditorias? entidad);
        Auditorias? Modificar(Auditorias? entidad);
        Auditorias? Borrar(Auditorias? entidad);
    }
}