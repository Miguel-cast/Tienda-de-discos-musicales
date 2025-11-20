using lib_dominio.Entidades;
using System.Collections.Generic;
using System.Diagnostics.Metrics;

namespace lib_repositorios.Interfaces
{
    public interface IClientesAplicacion
    {
        void Configurar(string StringConexion);
        List<Clientes> Listar();
        List<Clientes> PorNombre(Clientes? entidad);
        Clientes? Guardar(Clientes? entidad);
        Clientes? Modificar(Clientes? entidad);
        Clientes? Borrar(Clientes? entidad);
    }
}