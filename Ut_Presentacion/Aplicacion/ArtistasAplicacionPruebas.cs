using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class ArtistasAplicacionPruebas
    {
        private IConexion? iConexion;
        private ArtistasAplicacion? aplicacion;
        private Artistas? entidad;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new ArtistasAplicacion(iConexion);
        }

        [TestMethod]
        public void PruebaGuardarArtistaValido()
        {
            entidad = EntidadesNucleo.Artistas();
            entidad.NombreArtista = "Nuevo Artista";
            entidad.Nacionalidad = "Colombiana";
            var resultado = aplicacion!.Guardar(entidad);
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.ArtistaId > 0);
        }

        [TestMethod]
        public void PruebaValidacionNombreArtistaNulo()
        {
            entidad = EntidadesNucleo.Artistas();
            entidad.NombreArtista = null;
            entidad.Nacionalidad = "Colombiana";
            Assert.ThrowsException<Exception>(() => aplicacion!.Guardar(entidad));
        }

        [TestMethod]
        public void PruebaValidacionNacionalidadNula()
        {
            entidad = EntidadesNucleo.Artistas();
            entidad.NombreArtista = "Artista Prueba";
            entidad.Nacionalidad = null;
            var resultado = aplicacion!.Guardar(entidad);
            Assert.IsNotNull(resultado);
        }

        [TestCleanup]
        public void LimpiarDatos()
        {
            if (entidad != null && entidad.ArtistaId > 0)
            {
                var artistaExistente = iConexion!.Artistas!.Find(entidad.ArtistaId);
                if (artistaExistente != null)
                {
                    iConexion.Artistas.Remove(artistaExistente);
                    iConexion.SaveChanges();
                }
            }
        }
    }
}