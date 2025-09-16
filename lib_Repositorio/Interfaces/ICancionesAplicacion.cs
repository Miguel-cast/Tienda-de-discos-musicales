using lib_dominio.Entidades;
using System.Collections.Generic;

namespace lib_repositorios.Interfaces
{
    public interface ICancionesAplicacion
    {
        void Configurar(string StringConexion);
        List<Canciones> Listar();
        Canciones? Guardar(Canciones? entidad);
        Canciones? Modificar(Canciones? entidad);
        Canciones? Borrar(Canciones? entidad);
    }
}
