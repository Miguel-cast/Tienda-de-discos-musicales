using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class ProveedoresAplicacionPruebas
    {
        private IConexion? iConexion;
        private ProveedoresAplicacion? aplicacion;
        private Proveedores? entidad;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new ProveedoresAplicacion(iConexion);
        }

        [TestMethod]
        public void PruebaGuardarProveedorValido()
        {
            entidad = EntidadesNucleo.Proveedores();
            entidad.NombreEmpresa = "Proveedor S.A.";
            var resultado = aplicacion!.Guardar(entidad);
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.ProveedoresId > 0);
        }


        [TestCleanup]
        public void LimpiarDatos()
        {
            if (entidad != null && entidad.ProveedoresId > 0)
            {
                var proveedorExistente = iConexion!.Proveedores!.Find(entidad.ProveedoresId);
                if (proveedorExistente != null)
                {
                    iConexion.Proveedores.Remove(proveedorExistente);
                    iConexion.SaveChanges();
                }
            }
        }
    }
}