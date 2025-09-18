using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class GenerosAplicacionPruebas
    {
        private IConexion? iConexion;
        private GenerosAplicacion? aplicacion;
        private Generos? entidad;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new GenerosAplicacion(iConexion);
        }

        [TestMethod]
        public void PruebaGuardarGeneroValido()
        {
            entidad = EntidadesNucleo.Generos();
            entidad.NombreGenero = "Rock";
            var resultado = aplicacion!.Guardar(entidad);
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.GenerosId > 0);
        }

        
        [TestCleanup]
        public void LimpiarDatos()
        {
            if (entidad != null && entidad.GenerosId > 0)
            {
                var generoExistente = iConexion!.Generos!.Find(entidad.GenerosId);
                if (generoExistente != null)
                {
                    iConexion.Generos.Remove(generoExistente);
                    iConexion.SaveChanges();
                }
            }
        }
    }
}