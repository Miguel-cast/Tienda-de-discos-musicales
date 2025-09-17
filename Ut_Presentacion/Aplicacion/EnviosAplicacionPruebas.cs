using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class EnviosAplicacionPruebas
    {
        private IConexion? iConexion;
        private EnviosAplicacion? aplicacion;
        private Envios? entidad;
        private Pedidos? pedido;
        private Clientes? cliente;
        private Empleados? empleado;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new EnviosAplicacion(iConexion);

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

                // Crear empleado
                empleado = EntidadesNucleo.Empleados();
                iConexion!.Empleados!.Add(empleado);

                iConexion!.SaveChanges();

                // Crear pedido con estado "Completado"
                pedido = EntidadesNucleo.Pedidos();
                pedido.ClienteID = cliente.ClienteId;
                pedido.EmpleadoID = empleado.EmpleadoId;
                pedido.Estado = "Completado"; // Necesario para crear envío
                iConexion!.Pedidos!.Add(pedido);

                iConexion!.SaveChanges();
            }
            catch (Exception)
            {
                // Si ya existen, obtenerlos de la base de datos
                cliente = iConexion!.Clientes!.FirstOrDefault() ?? EntidadesNucleo.Clientes()!;
                empleado = iConexion!.Empleados!.FirstOrDefault() ?? EntidadesNucleo.Empleados()!;
                pedido = iConexion!.Pedidos!.FirstOrDefault() ?? EntidadesNucleo.Pedidos()!;

                // Asegurar que el pedido esté completado
                if (pedido != null)
                {
                    pedido.Estado = "Completado";
                    iConexion!.Pedidos!.Update(pedido);
                    iConexion!.SaveChanges();
                }
            }
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, PruebaGuardar());
            Assert.AreEqual(true, PruebaModificar());
            Assert.AreEqual(true, PruebaListar());
            Assert.AreEqual(true, PruebaObtenerEnviosPorCiudad());
            Assert.AreEqual(true, PruebaObtenerEnviosPorFecha());
            Assert.AreEqual(true, PruebaObtenerEnviosPendientes());
            Assert.AreEqual(true, PruebaObtenerEnviosRealizados());
            Assert.AreEqual(true, PruebaContarEnviosPorPais());
            Assert.AreEqual(true, PruebaObtenerEnvioPorPedido());
            Assert.AreEqual(true, PruebaBorrar());
        }

        public bool PruebaGuardar()
        {
            try
            {
                if (pedido == null || aplicacion == null)
                    return false;

                entidad = EntidadesNucleo.Envios();
                entidad.PedidoID = pedido.PedidoID;
                entidad = aplicacion.Guardar(entidad);
                return entidad != null && entidad.EnvioID > 0;
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

                entidad.CiudadEntrega = "Madrid";
                entidad = aplicacion.Modificar(entidad);
                return entidad != null && entidad.CiudadEntrega == "Madrid";
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

        public bool PruebaObtenerEnviosPorCiudad()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var envios = aplicacion.ObtenerEnviosPorCiudad("Madrid");
                return envios != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerEnviosPorFecha()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var fechaInicio = DateTime.Now.Date;
                var fechaFin = DateTime.Now.Date.AddDays(30);
                var envios = aplicacion.ObtenerEnviosPorFecha(fechaInicio, fechaFin);
                return envios != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerEnviosPendientes()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var enviosPendientes = aplicacion.ObtenerEnviosPendientes();
                return enviosPendientes != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerEnviosRealizados()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var enviosRealizados = aplicacion.ObtenerEnviosRealizados();
                return enviosRealizados != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaContarEnviosPorPais()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var contador = aplicacion.ContarEnviosPorPais("España");
                return contador >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerEnvioPorPedido()
        {
            try
            {
                if (aplicacion == null || pedido == null)
                    return false;

                var envio = aplicacion.ObtenerEnvioPorPedido(pedido.PedidoID);
                return envio != null;
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
        public void PruebaValidacionDireccionVacia()
        {
            if (aplicacion == null || pedido == null)
            {
                Assert.Fail("Datos de prueba no inicializados correctamente");
                return;
            }

            var envioInvalido = EntidadesNucleo.Envios();
            if (envioInvalido == null)
            {
                Assert.Fail("No se pudo crear envío de prueba");
                return;
            }

            envioInvalido.DireccionEntrega = "";
            envioInvalido.PedidoID = pedido.PedidoID;

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(envioInvalido));
        }

        [TestMethod]
        public void PruebaValidacionPedidoInexistente()
        {
            if (aplicacion == null)
            {
                Assert.Fail("Aplicación no inicializada correctamente");
                return;
            }

            var envioInvalido = EntidadesNucleo.Envios();
            if (envioInvalido == null)
            {
                Assert.Fail("No se pudo crear envío de prueba");
                return;
            }

            envioInvalido.PedidoID = 99999; // ID que no existe

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(envioInvalido));
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
                if (entidad != null && entidad.EnvioID > 0 && iConexion?.Envios != null)
                {
                    var envioExistente = iConexion.Envios.Find(entidad.EnvioID);
                    if (envioExistente != null)
                    {
                        iConexion.Envios.Remove(envioExistente);
                        iConexion.SaveChanges();
                    }
                }
            }
            catch { }
        }
    }
}