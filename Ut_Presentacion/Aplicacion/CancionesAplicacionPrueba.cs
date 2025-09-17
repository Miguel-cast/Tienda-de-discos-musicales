using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class CancionesAplicacionPrueba
    {
        private IConexion? iConexion;
        private CancionesAplicacion? aplicacion;
        private Canciones? entidad;
        private Discos? disco;
        private Artistas? artista;
        private Generos? genero;
        private Proveedores? proveedor;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new CancionesAplicacion(iConexion);

            // Crear datos de prueba necesarios
            CrearDatosPrueba();
        }

        private void CrearDatosPrueba()
        {
            try
            {
                // Crear artista
                artista = EntidadesNucleo.Artistas();
                iConexion!.Artistas!.Add(artista);

                // Crear género
                genero = EntidadesNucleo.Generos();
                iConexion!.Generos!.Add(genero);

                // Crear proveedor
                proveedor = EntidadesNucleo.Proveedores();
                iConexion!.Proveedores!.Add(proveedor);

                iConexion!.SaveChanges();

                // Crear disco
                disco = EntidadesNucleo.Discos();
                disco.ArtistaId = artista!.ArtistaId;
                disco.GenerosId = genero!.GenerosId;
                disco.ProveedoresId = proveedor!.ProveedoresId;
                iConexion!.Discos!.Add(disco);

                iConexion!.SaveChanges();
            }
            catch (Exception)
            {
                artista = iConexion!.Artistas!.FirstOrDefault() ?? EntidadesNucleo.Artistas()!;
                genero = iConexion!.Generos!.FirstOrDefault() ?? EntidadesNucleo.Generos()!;
                proveedor = iConexion!.Proveedores!.FirstOrDefault() ?? EntidadesNucleo.Proveedores()!;
                disco = iConexion!.Discos!.FirstOrDefault() ?? EntidadesNucleo.Discos()!;
            }
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, PruebaGuardar());
            Assert.AreEqual(true, PruebaModificar());
            Assert.AreEqual(true, PruebaListar());
            Assert.AreEqual(true, PruebaObtenerPorDisco());
            Assert.AreEqual(true, PruebaBuscarPorTitulo());
            Assert.AreEqual(true, PruebaBorrar());
        }

        public bool PruebaGuardar()
        {
            try
            {
                entidad = EntidadesNucleo.Canciones();
                entidad.Titulo = "Canción de prueba";
                entidad.DiscoID = disco!.DiscoId;

                entidad = aplicacion!.Guardar(entidad);
                return entidad != null && entidad.CancionId > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaModificar()
        {
            try
            {
                entidad!.Titulo = "Canción modificada";

                entidad = aplicacion!.Modificar(entidad);
                return entidad != null && entidad.Titulo == "Canción modificada";
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaListar()
        {
            try
            {
                var lista = aplicacion!.Listar();
                return lista != null && lista.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerPorDisco()
        {
            try
            {
                var canciones = aplicacion!.ObtenerPorDisco(disco!.DiscoId);
                return canciones != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaBuscarPorTitulo()
        {
            try
            {
                var canciones = aplicacion!.BuscarPorTitulo("Canción");
                return canciones != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaBorrar()
        {
            try
            {
                aplicacion!.Borrar(entidad);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 🔎 Pruebas de validaciones

        [TestMethod]
        public void PruebaValidacionTituloVacio()
        {
            var cancionInvalida = EntidadesNucleo.Canciones();
            cancionInvalida.Titulo = "";
            cancionInvalida.DiscoID = disco!.DiscoId;

            Assert.ThrowsException<Exception>(() => aplicacion!.Guardar(cancionInvalida));
        }



        [TestMethod]
        public void PruebaValidacionDiscoInexistente()
        {
            var cancionInvalida = EntidadesNucleo.Canciones();
            cancionInvalida.Titulo = "Canción inválida";
            cancionInvalida.DiscoID = 99999; // Id que no existe
            Assert.ThrowsException<Exception>(() => aplicacion!.Guardar(cancionInvalida));
        }

        [TestMethod]
        public void PruebaValidacionEntidadNula()
        {
            Assert.ThrowsException<Exception>(() => aplicacion!.Guardar(null));
            Assert.ThrowsException<Exception>(() => aplicacion!.Modificar(null));
            Assert.ThrowsException<Exception>(() => aplicacion!.Borrar(null));
        }

        [TestCleanup]
        public void LimpiarDatos()
        {
            try
            {
                if (entidad != null && entidad.CancionId > 0)
                {
                    var cancionExistente = iConexion!.Canciones!.Find(entidad.CancionId);
                    if (cancionExistente != null)
                    {
                        iConexion.Canciones.Remove(cancionExistente);
                        iConexion.SaveChanges();
                    }
                }
            }
            catch { }
        }
    }
}

