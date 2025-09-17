using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class ReseñasClientesAplicacionPruebas
    {
        private IConexion? iConexion;
        private ReseñasClientesAplicacion? aplicacion;
        private ReseñasClientes? entidad;
        private Clientes? cliente;
        private Discos? disco;
        private Artistas? artista;
        private Generos? genero;
        private Proveedores? proveedor;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new ReseñasClientesAplicacion(iConexion);

            // Crear datos de prueba necesarios
            CrearDatosPrueba();
        }

        private void CrearDatosPrueba()
        {
            try
            {
                // Crear cliente
                cliente = EntidadesNucleo.Clientes();
                iConexion!.Clientes!.Add(cliente);

                // Crear artista, género y proveedor para el disco
                artista = EntidadesNucleo.Artistas();
                iConexion!.Artistas!.Add(artista);

                genero = EntidadesNucleo.Generos();
                iConexion!.Generos!.Add(genero);

                proveedor = EntidadesNucleo.Proveedores();
                iConexion!.Proveedores!.Add(proveedor);

                iConexion!.SaveChanges();

                // Crear disco
                disco = EntidadesNucleo.Discos();
                disco.ArtistaId = artista.ArtistaId;
                disco.GenerosId = genero.GenerosId;
                disco.ProveedoresId = proveedor.ProveedoresId;
                iConexion!.Discos!.Add(disco);

                iConexion!.SaveChanges();
            }
            catch (Exception)
            {
                // Si ya existen, obtenerlos de la base de datos
                cliente = iConexion!.Clientes!.FirstOrDefault() ?? EntidadesNucleo.Clientes()!;
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
            Assert.AreEqual(true, PruebaObtenerReseñasPorDisco());
            Assert.AreEqual(true, PruebaObtenerReseñasPorCliente());
            Assert.AreEqual(true, PruebaCalcularPromedioCalificacionDisco());
            Assert.AreEqual(true, PruebaObtenerReseñasPorCalificacion());
            Assert.AreEqual(true, PruebaObtenerReseñasRecientes());
            Assert.AreEqual(true, PruebaContarReseñasPorDisco());
            Assert.AreEqual(true, PruebaBorrar());
        }

        public bool PruebaGuardar()
        {
            try
            {
                if (cliente == null || disco == null || aplicacion == null)
                    return false;

                entidad = EntidadesNucleo.ReseñasClientes();
                entidad.ClienteID = cliente.ClienteId;
                entidad.DiscoID = disco.DiscoId;
                entidad = aplicacion.Guardar(entidad);
                return entidad != null && entidad.ReseñaID > 0;
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
                if (entidad == null || aplicacion == null)
                    return false;

                entidad.Calificacion = 4;
                entidad.Comentario = "Modificado: Muy buen álbum, me gustó mucho la calidad del sonido.";
                entidad = aplicacion.Modificar(entidad);
                return entidad != null && entidad.Calificacion == 4;
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
                if (aplicacion == null)
                    return false;

                var lista = aplicacion.Listar();
                return lista != null && lista.Count > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerReseñasPorDisco()
        {
            try
            {
                if (aplicacion == null || disco == null)
                    return false;

                var reseñas = aplicacion.ObtenerReseñasPorDisco(disco.DiscoId);
                return reseñas != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerReseñasPorCliente()
        {
            try
            {
                if (aplicacion == null || cliente == null)
                    return false;

                var reseñas = aplicacion.ObtenerReseñasPorCliente(cliente.ClienteId);
                return reseñas != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaCalcularPromedioCalificacionDisco()
        {
            try
            {
                if (aplicacion == null || disco == null)
                    return false;

                var promedio = aplicacion.CalcularPromedioCalificacionDisco(disco.DiscoId);
                return promedio >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerReseñasPorCalificacion()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var reseñas = aplicacion.ObtenerReseñasPorCalificacion(5);
                return reseñas != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerReseñasRecientes()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var reseñasRecientes = aplicacion.ObtenerReseñasRecientes();
                return reseñasRecientes != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaContarReseñasPorDisco()
        {
            try
            {
                if (aplicacion == null || disco == null)
                    return false;

                var contador = aplicacion.ContarReseñasPorDisco(disco.DiscoId);
                return contador >= 0;
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
                if (aplicacion == null || entidad == null)
                    return false;

                aplicacion.Borrar(entidad);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [TestMethod]
        public void PruebaValidacionCalificacionInvalida()
        {
            if (aplicacion == null || cliente == null || disco == null)
            {
                Assert.Fail("Datos de prueba no inicializados correctamente");
                return;
            }

            var reseñaInvalida = EntidadesNucleo.ReseñasClientes();
            if (reseñaInvalida == null)
            {
                Assert.Fail("No se pudo crear reseña de prueba");
                return;
            }

            reseñaInvalida.Calificacion = 6; // Calificación inválida
            reseñaInvalida.ClienteID = cliente.ClienteId;
            reseñaInvalida.DiscoID = disco.DiscoId;

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(reseñaInvalida));
        }

        [TestMethod]
        public void PruebaValidacionComentarioVacio()
        {
            if (aplicacion == null || cliente == null || disco == null)
            {
                Assert.Fail("Datos de prueba no inicializados correctamente");
                return;
            }

            var reseñaInvalida = EntidadesNucleo.ReseñasClientes();
            if (reseñaInvalida == null)
            {
                Assert.Fail("No se pudo crear reseña de prueba");
                return;
            }

            reseñaInvalida.Comentario = "";
            reseñaInvalida.ClienteID = cliente.ClienteId;
            reseñaInvalida.DiscoID = disco.DiscoId;

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(reseñaInvalida));
        }

        [TestMethod]
        public void PruebaValidacionComentarioMuyCorto()
        {
            if (aplicacion == null || cliente == null || disco == null)
            {
                Assert.Fail("Datos de prueba no inicializados correctamente");
                return;
            }

            var reseñaInvalida = EntidadesNucleo.ReseñasClientes();
            if (reseñaInvalida == null)
            {
                Assert.Fail("No se pudo crear reseña de prueba");
                return;
            }

            reseñaInvalida.Comentario = "Corto"; // Muy corto
            reseñaInvalida.ClienteID = cliente.ClienteId;
            reseñaInvalida.DiscoID = disco.DiscoId;

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(reseñaInvalida));
        }

        [TestMethod]
        public void PruebaValidacionEntidadNula()
        {
            if (aplicacion == null)
            {
                Assert.Fail("Aplicación no inicializada correctamente");
                return;
            }

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(null));
            Assert.ThrowsException<Exception>(() => aplicacion.Modificar(null));
            Assert.ThrowsException<Exception>(() => aplicacion.Borrar(null));
        }

        [TestCleanup]
        public void LimpiarDatos()
        {
            try
            {
                if (entidad != null && entidad.ReseñaID > 0 && iConexion?.ReseñasClientes != null)
                {
                    var reseñaExistente = iConexion.ReseñasClientes.Find(entidad.ReseñaID);
                    if (reseñaExistente != null)
                    {
                        iConexion.ReseñasClientes.Remove(reseñaExistente);
                        iConexion.SaveChanges();
                    }
                }
            }
            catch { }
        }
    }
}