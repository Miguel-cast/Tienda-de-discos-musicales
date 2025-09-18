using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class EmpleadosAplicacionPruebas
    {
        private IConexion? iConexion;
        private EmpleadosAplicacion? aplicacion;
        private Empleados? entidad;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new EmpleadosAplicacion(iConexion);
        }

        [TestMethod]
        public void PruebaGuardarEmpleadoValido()
        {
            entidad = EntidadesNucleo.Empleados();
            entidad.Nombre = "Empleado Prueba";
            entidad.Cargo = "Vendedor";
            var resultado = aplicacion!.Guardar(entidad);
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.EmpleadoId > 0);
        }


        [TestMethod]
        public void PruebaValidacionCargoNulo()
        {
            entidad = EntidadesNucleo.Empleados();
            entidad.Nombre = "Empleado Prueba";
            entidad.Cargo = null;
            var resultado = aplicacion!.Guardar(entidad);
            Assert.IsNotNull(resultado);
        }

        [TestCleanup]
        public void LimpiarDatos()
        {
            if (entidad != null && entidad.EmpleadoId > 0)
            {
                var empleadoExistente = iConexion!.Empleados!.Find(entidad.EmpleadoId);
                if (empleadoExistente != null)
                {
                    iConexion.Empleados.Remove(empleadoExistente);
                    iConexion.SaveChanges();
                }
            }
        }
    }
}
