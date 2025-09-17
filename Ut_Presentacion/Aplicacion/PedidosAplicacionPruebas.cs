using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class PedidosAplicacionPruebas
    {
        private IConexion? iConexion;
        private PedidosAplicacion? aplicacion;
        private Pedidos? entidad;
        private Clientes? cliente;
        private Empleados? empleado;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new PedidosAplicacion(iConexion);

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
            }
            catch (Exception)
            {
                // Si ya existen, obtenerlos de la base de datos
                cliente = iConexion!.Clientes!.FirstOrDefault() ?? EntidadesNucleo.Clientes()!;
                empleado = iConexion!.Empleados!.FirstOrDefault() ?? EntidadesNucleo.Empleados()!;
            }
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, PruebaGuardar());
            Assert.AreEqual(true, PruebaModificar());
            Assert.AreEqual(true, PruebaListar());
            Assert.AreEqual(true, PruebaObtenerPedidosPorCliente());
            Assert.AreEqual(true, PruebaObtenerPedidosPorEstado());
            Assert.AreEqual(true, PruebaContarPedidosPorMes());
            Assert.AreEqual(true, PruebaObtenerPedidosRecientes());
            Assert.AreEqual(true, PruebaBorrar());
        }

        public bool PruebaGuardar()
        {
            try
            {
                if (cliente == null || empleado == null || aplicacion == null)
                    return false;

                entidad = EntidadesNucleo.Pedidos();
                entidad.ClienteID = cliente.ClienteId;
                entidad.EmpleadoID = empleado.EmpleadoId;

                entidad = aplicacion.Guardar(entidad);
                return entidad != null && entidad.PedidoID > 0;
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

                entidad.Estado = "Completado";
                entidad = aplicacion.Modificar(entidad);
                return entidad != null && entidad.Estado == "Completado";
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

        public bool PruebaObtenerPedidosPorCliente()
        {
            try
            {
                if (aplicacion == null || cliente == null)
                    return false;

                var pedidos = aplicacion.ObtenerPedidosPorCliente(cliente.ClienteId);
                return pedidos != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerPedidosPorEstado()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var pedidos = aplicacion.ObtenerPedidosPorEstado("Pendiente");
                return pedidos != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaContarPedidosPorMes()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var contador = aplicacion.ContarPedidosPorMes(DateTime.Now.Year, DateTime.Now.Month);
                return contador >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerPedidosRecientes()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var pedidosRecientes = aplicacion.ObtenerPedidosRecientes();
                return pedidosRecientes != null;
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
        public void PruebaValidacionFechaFutura()
        {
            if (aplicacion == null || cliente == null || empleado == null)
            {
                Assert.Fail("Datos de prueba no inicializados correctamente");
                return;
            }

            var pedidoInvalido = EntidadesNucleo.Pedidos();
            if (pedidoInvalido == null)
            {
                Assert.Fail("No se pudo crear pedido de prueba");
                return;
            }

            pedidoInvalido.FechaPedido = DateTime.Now.Date.AddDays(1); // Fecha futura
            pedidoInvalido.ClienteID = cliente.ClienteId;
            pedidoInvalido.EmpleadoID = empleado.EmpleadoId;

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(pedidoInvalido));
        }

        [TestMethod]
        public void PruebaValidacionEstadoInvalido()
        {
            if (aplicacion == null || cliente == null || empleado == null)
            {
                Assert.Fail("Datos de prueba no inicializados correctamente");
                return;
            }

            var pedidoInvalido = EntidadesNucleo.Pedidos();
            if (pedidoInvalido == null)
            {
                Assert.Fail("No se pudo crear pedido de prueba");
                return;
            }

            pedidoInvalido.Estado = "EstadoInvalido";
            pedidoInvalido.ClienteID = cliente.ClienteId;
            pedidoInvalido.EmpleadoID = empleado.EmpleadoId;

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(pedidoInvalido));
        }

        [TestMethod]
        public void PruebaValidacionClienteInexistente()
        {
            if (aplicacion == null || empleado == null)
            {
                Assert.Fail("Datos de prueba no inicializados correctamente");
                return;
            }

            var pedidoInvalido = EntidadesNucleo.Pedidos();
            if (pedidoInvalido == null)
            {
                Assert.Fail("No se pudo crear pedido de prueba");
                return;
            }

            pedidoInvalido.ClienteID = 99999; // ID que no existe
            pedidoInvalido.EmpleadoID = empleado.EmpleadoId;

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(pedidoInvalido));
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
                if (entidad != null && entidad.PedidoID > 0 && iConexion?.Pedidos != null)
                {
                    var pedidoExistente = iConexion.Pedidos.Find(entidad.PedidoID);
                    if (pedidoExistente != null)
                    {
                        iConexion.Pedidos.Remove(pedidoExistente);
                        iConexion.SaveChanges();
                    }
                }
            }
            catch { }
        }
    }
}