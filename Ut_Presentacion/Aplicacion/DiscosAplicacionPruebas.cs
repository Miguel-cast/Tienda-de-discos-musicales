using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class DiscosAplicacionPruebas
    {
        private IConexion? iConexion;
        private DiscosAplicacion? aplicacion;
        private Discos? entidad;
        private Artistas? artista;
        private Generos? genero;
        private Proveedores? proveedor;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new DiscosAplicacion(iConexion);

            CrearDatosPrueba();
        }

        private void CrearDatosPrueba()
        {
            try
            {
                artista = EntidadesNucleo.Artistas();
                iConexion!.Artistas!.Add(artista);

                genero = EntidadesNucleo.Generos();
                iConexion!.Generos!.Add(genero);

                proveedor = EntidadesNucleo.Proveedores();
                iConexion!.Proveedores!.Add(proveedor);

                iConexion!.SaveChanges();
            }
            catch (Exception)
            {
                artista = iConexion!.Artistas!.FirstOrDefault() ?? EntidadesNucleo.Artistas()!;
                genero = iConexion!.Generos!.FirstOrDefault() ?? EntidadesNucleo.Generos()!;
                proveedor = iConexion!.Proveedores!.FirstOrDefault() ?? EntidadesNucleo.Proveedores()!;
            }
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, PruebaGuardar());
            Assert.AreEqual(true, PruebaModificar());
            Assert.AreEqual(true, PruebaListar());
            Assert.AreEqual(true, PruebaObtenerDiscosPorArtista());
            Assert.AreEqual(true, PruebaObtenerDiscosPorGenero());
            Assert.AreEqual(true, PruebaCalcularPromedioPrecios());
            Assert.AreEqual(true, PruebaObtenerDiscosRecientes());
            Assert.AreEqual(true, PruebaBorrar());
        }

        public bool PruebaGuardar()
        {
            try
            {
                entidad = EntidadesNucleo.Discos();
                entidad.ArtistaId = artista!.ArtistaId;
                entidad.GenerosId = genero!.GenerosId;
                entidad.ProveedoresId = proveedor!.ProveedoresId;

                entidad = aplicacion!.Guardar(entidad);
                return entidad != null && entidad.DiscoId > 0;
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
                entidad!.Precio = 75000.00m;
                entidad!.AñoLanzamiento = 2024;
                entidad = aplicacion!.Modificar(entidad);
                return entidad != null && entidad.Precio == 75000.00m && entidad.AñoLanzamiento == 2024;
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

        public bool PruebaObtenerDiscosPorArtista()
        {
            try
            {
                var discos = aplicacion!.ObtenerDiscosPorArtista(artista!.ArtistaId);
                return discos != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerDiscosPorGenero()
        {
            try
            {
                var discos = aplicacion!.ObtenerDiscosPorGenero(genero!.GenerosId);
                return discos != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaCalcularPromedioPrecios()
        {
            try
            {
                var promedio = aplicacion!.CalcularPromedioPrecios();
                return promedio >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerDiscosRecientes()
        {
            try
            {
                var discosRecientes = aplicacion!.ObtenerDiscosRecientes();
                return discosRecientes != null;
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

        [TestMethod]
        public void PruebaValidacionPrecioCero()
        {
            var discoInvalido = EntidadesNucleo.Discos();
            discoInvalido.Precio = 0;
            discoInvalido.ArtistaId = artista!.ArtistaId;
            discoInvalido.GenerosId = genero!.GenerosId;
            discoInvalido.ProveedoresId = proveedor!.ProveedoresId; // Corregido

            Assert.ThrowsException<Exception>(() => aplicacion!.Guardar(discoInvalido));
        }

        [TestMethod]
        public void PruebaValidacionAñoInvalido()
        {
            var discoInvalido = EntidadesNucleo.Discos();
            discoInvalido.AñoLanzamiento = 1800; // Año muy antiguo
            discoInvalido.ArtistaId = artista!.ArtistaId;
            discoInvalido.GenerosId = genero!.GenerosId;
            discoInvalido.ProveedoresId = proveedor!.ProveedoresId; // Corregido

            Assert.ThrowsException<Exception>(() => aplicacion!.Guardar(discoInvalido));
        }

        [TestMethod]
        public void PruebaValidacionArtistaInexistente()
        {
            var discoInvalido = EntidadesNucleo.Discos();
            discoInvalido.ArtistaId = 99999; // ID que no existe
            discoInvalido.GenerosId = genero!.GenerosId;
            discoInvalido.ProveedoresId = proveedor!.ProveedoresId; // Corregido

            Assert.ThrowsException<Exception>(() => aplicacion!.Guardar(discoInvalido));
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
                if (entidad != null && entidad.DiscoId > 0)
                {
                    var discoExistente = iConexion!.Discos!.Find(entidad.DiscoId);
                    if (discoExistente != null)
                    {
                        iConexion.Discos.Remove(discoExistente);
                        iConexion.SaveChanges();
                    }
                }
            }
            catch { }
        }
    }
}