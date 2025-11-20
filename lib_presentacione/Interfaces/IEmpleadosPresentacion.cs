using lib_dominio.Entidades;
using System.Diagnostics.Metrics;

namespace lib_presentaciones.Interfaces
{
    public interface IEmpleadosPresentacion
    {
        Task<List<Empleados>> Listar();
        Task<List<Empleados>> PorCargo(Empleados? entidad);
        Task<Empleados?> Guardar(Empleados? entidad);
        Task<Empleados?> Modificar(Empleados? entidad);
        Task<Empleados?> Borrar(Empleados? entidad);
    }
}