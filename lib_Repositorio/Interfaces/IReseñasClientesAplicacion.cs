using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IReseñasClientesAplicacion
    {
        void Configurar(string StringConexion);
        List<ReseñasClientes> Listar();
        ReseñasClientes? Guardar(ReseñasClientes? entidad);
        ReseñasClientes? Modificar(ReseñasClientes? entidad);
        ReseñasClientes? Borrar(ReseñasClientes? entidad);
        List<ReseñasClientes> ObtenerReseñasPorDisco(int discoId);
        List<ReseñasClientes> ObtenerReseñasPorCliente(int clienteId);
        double CalcularPromedioCalificacionDisco(int discoId);
        List<ReseñasClientes> ObtenerReseñasPorCalificacion(int calificacion);
        List<ReseñasClientes> ObtenerReseñasRecientes(int dias = 7);
        int ContarReseñasPorDisco(int discoId);
    }
}