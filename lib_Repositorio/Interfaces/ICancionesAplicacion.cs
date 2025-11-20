using lib_dominio.Entidades;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace lib_repositorios.Interfaces
{
    public interface ICancionesAplicacion
    {
        void Configurar(string StringConexion);
        List<Canciones> Listar();
        List<Canciones> PorTitulo(Canciones? entidad);
        Canciones? Guardar(Canciones? entidad);
        Canciones? Modificar(Canciones? entidad);
        Canciones? Borrar(Canciones? entidad);
    }
}
