using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class PagosAplicacionPruebas
    {
        private IConexion? iConexion;
        private PagosAplicacion? aplicacion;
        private Pagos? entidad;
        private Facturas? factura;
        private Pedidos? pedido;
        private Clientes? cliente;
        private Empleados? empleado;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new PagosAplicacion(iConexion);

            CrearDatosPrueba();
        }

        private void CrearDatosPrueba()
        {
            try
            {
                cliente = EntidadesNucleo.Clientes();
                iConexion!.Clientes!.Add(cliente);

                empleado = EntidadesNucleo.Empleados();
                iConexion!.Empleados!.Add(empleado);

                iConexion!.SaveChanges();

                pedido = EntidadesNucleo.Pedidos();
                pedido.ClienteID = cliente.ClienteId;
                pedido.EmpleadoID = empleado.EmpleadoId;
                iConexion!.Pedidos!.Add(pedido);

                iConexion!.SaveChanges();

                factura = EntidadesNucleo.Facturas();
                factura.PedidoID = pedido.PedidoID;
                iConexion!.Facturas!.Add(factura);

                iConexion!.SaveChanges();
            }
            catch (Exception)
            {
                cliente = iConexion!.Clientes!.FirstOrDefault() ?? EntidadesNucleo.Clientes()!;
                empleado = iConexion!.Empleados!.FirstOrDefault() ?? EntidadesNucleo.Empleados()!;
                pedido = iConexion!.Pedidos!.FirstOrDefault() ?? EntidadesNucleo.Pedidos()!;
                factura = iConexion!.Facturas!.FirstOrDefault() ?? EntidadesNucleo.Facturas()!;
            }
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, PruebaGuardar());
            Assert.AreEqual(true, PruebaModificar());
            Assert.AreEqual(true, PruebaListar());
            Assert.AreEqual(true, PruebaObtenerPagosPorFactura());
            Assert.AreEqual(true, PruebaCalcularTotalPagosPorMetodo());
            Assert.AreEqual(true, PruebaObtenerPagosPorFecha());
            Assert.AreEqual(true, PruebaCalcularSaldoPendienteFactura());
            Assert.AreEqual(true, PruebaObtenerPagosRecientes());
            Assert.AreEqual(true, PruebaBorrar());
        }

        public bool PruebaGuardar()
        {
            try
            {
                if (factura == null || aplicacion == null)
                    return false;

                entidad = EntidadesNucleo.Pagos();
                entidad.FacturaID = factura.FacturaID;
                entidad = aplicacion.Guardar(entidad);
                return entidad != null && entidad.PagoID > 0;
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

                entidad.Monto = 50000.00m;
                entidad = aplicacion.Modificar(entidad);
                return entidad != null && entidad.Monto == 50000.00m;
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

        public bool PruebaObtenerPagosPorFactura()
        {
            try
            {
                if (aplicacion == null || factura == null)
                    return false;

                var pagos = aplicacion.ObtenerPagosPorFactura(factura.FacturaID);
                return pagos != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaCalcularTotalPagosPorMetodo()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var total = aplicacion.CalcularTotalPagosPorMetodo("Tarjeta de Crédito");
                return total >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerPagosPorFecha()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var fechaInicio = DateTime.Now.Date.AddDays(-30);
                var fechaFin = DateTime.Now.Date;
                var pagos = aplicacion.ObtenerPagosPorFecha(fechaInicio, fechaFin);
                return pagos != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaCalcularSaldoPendienteFactura()
        {
            try
            {
                if (aplicacion == null || factura == null)
                    return false;

                var saldo = aplicacion.CalcularSaldoPendienteFactura(factura.FacturaID);
                return saldo >= 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaObtenerPagosRecientes()
        {
            try
            {
                if (aplicacion == null)
                    return false;

                var pagosRecientes = aplicacion.ObtenerPagosRecientes();
                return pagosRecientes != null;
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
        public void PruebaValidacionMontoCero()
        {
            if (aplicacion == null || factura == null)
            {
                Assert.Fail("Datos de prueba no inicializados correctamente");
                return;
            }

            var pagoInvalido = EntidadesNucleo.Pagos();
            if (pagoInvalido == null)
            {
                Assert.Fail("No se pudo crear pago de prueba");
                return;
            }

            pagoInvalido.Monto = 0;
            pagoInvalido.FacturaID = factura.FacturaID;

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(pagoInvalido));
        }

        [TestMethod]
        public void PruebaValidacionMetodoPagoInvalido()
        {
            if (aplicacion == null || factura == null)
            {
                Assert.Fail("Datos de prueba no inicializados correctamente");
                return;
            }

            var pagoInvalido = EntidadesNucleo.Pagos();
            if (pagoInvalido == null)
            {
                Assert.Fail("No se pudo crear pago de prueba");
                return;
            }

            pagoInvalido.MetodoPago = "MetodoInvalido";
            pagoInvalido.FacturaID = factura.FacturaID;

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(pagoInvalido));
        }

        [TestMethod]
        public void PruebaValidacionFacturaInexistente()
        {
            if (aplicacion == null)
            {
                Assert.Fail("Aplicación no inicializada correctamente");
                return;
            }

            var pagoInvalido = EntidadesNucleo.Pagos();
            if (pagoInvalido == null)
            {
                Assert.Fail("No se pudo crear pago de prueba");
                return;
            }

            pagoInvalido.FacturaID = 99999; // ID que no existe

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(pagoInvalido));
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
                if (entidad != null && entidad.PagoID > 0 && iConexion?.Pagos != null)
                {
                    var pagoExistente = iConexion.Pagos.Find(entidad.PagoID);
                    if (pagoExistente != null)
                    {
                        iConexion.Pagos.Remove(pagoExistente);
                        iConexion.SaveChanges();
                    }
                }
            }
            catch { }
        }
    }
}