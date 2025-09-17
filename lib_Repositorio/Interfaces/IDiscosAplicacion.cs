using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_Repositorio.Interfaces
{
    public interface IDiscosAplicacion
    {
        void Configurar(string StringConexion);
        List<Discos> Listar();
        Discos? Guardar(Discos? entidad);
        Discos? Modificar(Discos? entidad);
        Discos? Borrar(Discos? entidad);
        List<Discos> ObtenerDiscosPorArtista(int artistaId);
        List<Discos> ObtenerDiscosPorGenero(int generoId);
        decimal CalcularPromedioPrecios();
        List<Discos> ObtenerDiscosRecientes(int añosAtras = 5);
    }
}
