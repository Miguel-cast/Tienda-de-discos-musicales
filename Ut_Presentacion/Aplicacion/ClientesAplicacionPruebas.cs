using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class ClientesAplicacionPruebas
    {
        private IConexion? iConexion;
        private ClientesAplicacion? aplicacion;
        private Clientes? entidad;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new ClientesAplicacion(iConexion);
        }

        [TestMethod]
        public void PruebaGuardarClienteValido()
        {
            entidad = EntidadesNucleo.Clientes();
            entidad.Nombre = "Juan";
            entidad.Apellido = "Pérez";
            entidad.Email = "juan@email.com";
            var resultado = aplicacion!.Guardar(entidad);
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.ClienteId > 0);
        }


        [TestCleanup]
        public void LimpiarDatos()
        {
            if (entidad != null && entidad.ClienteId > 0)
            {
                var clienteExistente = iConexion!.Clientes!.Find(entidad.ClienteId);
                if (clienteExistente != null)
                {
                    iConexion.Clientes.Remove(clienteExistente);
                    iConexion.SaveChanges();
                }
            }
        }
    }
}