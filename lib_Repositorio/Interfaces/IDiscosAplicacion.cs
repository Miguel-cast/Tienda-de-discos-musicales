using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace lib_repositorios.Interfaces
{
    public interface IDiscosAplicacion
    {
        void Configurar(string StringConexion);
        List<Discos> Listar();
        List<Discos> PorTitulo (Discos? entidad);
        Discos? Guardar(Discos? entidad);
        Discos? Modificar(Discos? entidad);
        Discos? Borrar(Discos? entidad);


    }
}
