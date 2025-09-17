using lib_dominio.Entidades;
using System.Collections.Generic;

namespace lib_repositorios.Interfaces
{
    public interface IGenerosAplicacion
    {
        void Configurar(string StringConexion);
        List<Generos> Listar();
        Generos? Guardar(Generos? entidad);
        Generos? Modificar(Generos? entidad);
        Generos? Borrar(Generos? entidad);
    }
}