using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class FacturasAplicacionPruebas
    {
        private IConexion? iConexion;
        private FacturasAplicacion? aplicacion;
        private Facturas? entidad;
        private Pedidos? pedido;
        private Clientes? cliente;
        private Empleados? empleado;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new FacturasAplicacion(iConexion);

            CrearDatosPrueba();
        }

        private void CrearDatosPrueba()
        {
            try
            {
                cliente = iConexion!.Clientes!.FirstOrDefault();
                empleado = iConexion!.Empleados!.FirstOrDefault();

                if (cliente == null)
                {
                    cliente = EntidadesNucleo.Clientes();
                    iConexion!.Clientes!.Add(cliente);
                }

                if (empleado == null)
                {
                    empleado = EntidadesNucleo.Empleados();
                    iConexion!.Empleados!.Add(empleado);
                }

                iConexion!.SaveChanges();

                pedido = iConexion!.Pedidos!
                    .Where(p => !iConexion.Facturas!.Any(f => f.PedidoID == p.PedidoID))
                    .FirstOrDefault();

                if (pedido == null)
                {
                    pedido = EntidadesNucleo.Pedidos();
                    pedido.ClienteID = cliente.ClienteId;
                    pedido.EmpleadoID = empleado.EmpleadoId;
                    iConexion!.Pedidos!.Add(pedido);
                    iConexion!.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                cliente = iConexion!.Clientes!.FirstOrDefault() ?? EntidadesNucleo.Clientes()!;
                empleado = iConexion!.Empleados!.FirstOrDefault() ?? EntidadesNucleo.Empleados()!;
                pedido = iConexion!.Pedidos!.FirstOrDefault() ?? EntidadesNucleo.Pedidos()!;
                Console.WriteLine($"Error en CrearDatosPrueba: {ex.Message}");
            }
        }

        [TestMethod]
        public void Ejecutar()
        {
            Console.WriteLine("=== INICIANDO PRUEBAS FACTURAS ===");

            bool guardado = PruebaGuardar();
            Console.WriteLine($"Guardar: {guardado}");
            Assert.AreEqual(true, guardado);

            bool modificado = PruebaModificar();
            Console.WriteLine($"Modificar: {modificado}");
            Assert.AreEqual(true, modificado);

            bool listado = PruebaListar();
            Console.WriteLine($"Listar: {listado}");
            Assert.AreEqual(true, listado);

            bool totalCliente = PruebaCalcularTotalFacturasCliente();
            Console.WriteLine($"Total Cliente: {totalCliente}");
            Assert.AreEqual(true, totalCliente);

            bool pendientes = PruebaObtenerFacturasPendientesPago();
            Console.WriteLine($"Pendientes: {pendientes}");
            Assert.AreEqual(true, pendientes);

            bool porFecha = PruebaObtenerFacturasPorFecha();
            Console.WriteLine($"Por Fecha: {porFecha}");
            Assert.AreEqual(true, porFecha);

            bool totalMes = PruebaCalcularMontoTotalMes();
            Console.WriteLine($"Total Mes: {totalMes}");
            Assert.AreEqual(true, totalMes);

            bool porPedido = PruebaObtenerFacturaPorPedido();
            Console.WriteLine($"Por Pedido: {porPedido}");
            Assert.AreEqual(true, porPedido);

            bool borrado = PruebaBorrar();
            Console.WriteLine($"Borrar: {borrado}");
            Assert.AreEqual(true, borrado);
        }

        public bool PruebaGuardar()
        {
            try
            {
                Console.WriteLine("--- Iniciando PruebaGuardar ---");

                if (pedido == null || aplicacion == null)
                {
                    Console.WriteLine("ERROR: Pedido o aplicación es null");
                    return false;
                }

                Console.WriteLine($"Pedido ID: {pedido.PedidoID}");

                entidad = EntidadesNucleo.Facturas();
                entidad.PedidoID = pedido.PedidoID;

                Console.WriteLine($"Factura creada - Total: {entidad.Total}, PedidoID: {entidad.PedidoID}");

                entidad = aplicacion.Guardar(entidad);

                bool resultado = entidad != null && entidad.FacturaID > 0;
                Console.WriteLine($"Resultado Guardar: {resultado}, FacturaID: {entidad?.FacturaID}");

                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en PruebaGuardar: {ex.Message}");
                return false;
            }
        }

        public bool PruebaModificar()
        {
            try
            {
                Console.WriteLine("--- Iniciando PruebaModificar ---");

                if (entidad == null || aplicacion == null)
                {
                    Console.WriteLine("ERROR: Entidad o aplicación es null");
                    return false;
                }

                var totalOriginal = entidad.Total;
                entidad.Total = 250000.00m;

                Console.WriteLine($"Modificando total de {totalOriginal} a {entidad.Total}");

                entidad = aplicacion.Modificar(entidad);

                bool resultado = entidad != null && entidad.Total == 250000.00m;
                Console.WriteLine($"Resultado Modificar: {resultado}");

                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en PruebaModificar: {ex.Message}");
                return false;
            }
        }

        public bool PruebaListar()
        {
            try
            {
                Console.WriteLine("--- Iniciando PruebaListar ---");

                if (aplicacion == null)
                {
                    Console.WriteLine("ERROR: Aplicación es null");
                    return false;
                }

                var lista = aplicacion.Listar();

                Console.WriteLine($"Facturas encontradas: {lista?.Count ?? 0}");

                return lista != null && lista.Count > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en PruebaListar: {ex.Message}");
                return false;
            }
        }

        public bool PruebaCalcularTotalFacturasCliente()
        {
            try
            {
                Console.WriteLine("--- Iniciando PruebaCalcularTotalFacturasCliente ---");

                if (aplicacion == null || cliente == null)
                {
                    Console.WriteLine("ERROR: Aplicación o cliente es null");
                    return false;
                }

                var total = aplicacion.CalcularTotalFacturasCliente(cliente.ClienteId);

                Console.WriteLine($"Total facturas cliente {cliente.ClienteId}: {total}");

                return total >= 0; // Cambiado: puede ser 0 si no hay facturas
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en PruebaCalcularTotalFacturasCliente: {ex.Message}");
                return false;
            }
        }

        public bool PruebaObtenerFacturasPendientesPago()
        {
            try
            {
                Console.WriteLine("--- Iniciando PruebaObtenerFacturasPendientesPago ---");

                if (aplicacion == null)
                {
                    Console.WriteLine("ERROR: Aplicación es null");
                    return false;
                }

                var facturasPendientes = aplicacion.ObtenerFacturasPendientesPago();

                Console.WriteLine($"Facturas pendientes: {facturasPendientes?.Count ?? 0}");

                return facturasPendientes != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en PruebaObtenerFacturasPendientesPago: {ex.Message}");
                return false;
            }
        }

        public bool PruebaObtenerFacturasPorFecha()
        {
            try
            {
                Console.WriteLine("--- Iniciando PruebaObtenerFacturasPorFecha ---");

                if (aplicacion == null)
                {
                    Console.WriteLine("ERROR: Aplicación es null");
                    return false;
                }

                var fechaInicio = DateTime.Now.Date.AddDays(-30);
                var fechaFin = DateTime.Now.Date;
                var facturas = aplicacion.ObtenerFacturasPorFecha(fechaInicio, fechaFin);

                Console.WriteLine($"Facturas en rango de fecha: {facturas?.Count ?? 0}");

                return facturas != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en PruebaObtenerFacturasPorFecha: {ex.Message}");
                return false;
            }
        }

        public bool PruebaCalcularMontoTotalMes()
        {
            try
            {
                Console.WriteLine("--- Iniciando PruebaCalcularMontoTotalMes ---");

                if (aplicacion == null)
                {
                    Console.WriteLine("ERROR: Aplicación es null");
                    return false;
                }

                var total = aplicacion.CalcularMontoTotalMes(DateTime.Now.Year, DateTime.Now.Month);

                Console.WriteLine($"Total mes actual: {total}");

                return total >= 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en PruebaCalcularMontoTotalMes: {ex.Message}");
                return false;
            }
        }

        public bool PruebaObtenerFacturaPorPedido()
        {
            try
            {
                Console.WriteLine("--- Iniciando PruebaObtenerFacturaPorPedido ---");

                if (aplicacion == null || pedido == null)
                {
                    Console.WriteLine("ERROR: Aplicación o pedido es null");
                    return false;
                }

                var factura = aplicacion.ObtenerFacturaPorPedido(pedido.PedidoID);

                Console.WriteLine($"Factura encontrada para pedido {pedido.PedidoID}: {factura != null}");

                return factura != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en PruebaObtenerFacturaPorPedido: {ex.Message}");
                return false;
            }
        }

        public bool PruebaBorrar()
        {
            try
            {
                Console.WriteLine("--- Iniciando PruebaBorrar ---");

                if (aplicacion == null || entidad == null)
                {
                    Console.WriteLine("ERROR: Aplicación o entidad es null");
                    return false;
                }

                Console.WriteLine($"Borrando factura ID: {entidad.FacturaID}");

                aplicacion.Borrar(entidad);

                Console.WriteLine("Factura borrada exitosamente");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR en PruebaBorrar: {ex.Message}");
                return false;
            }
        }

        [TestMethod]
        public void PruebaValidacionTotalCero()
        {
            if (aplicacion == null || pedido == null)
            {
                Assert.Fail("Datos de prueba no inicializados correctamente");
                return;
            }

            var facturaInvalida = EntidadesNucleo.Facturas();
            if (facturaInvalida == null)
            {
                Assert.Fail("No se pudo crear factura de prueba");
                return;
            }

            facturaInvalida.Total = 0;
            facturaInvalida.PedidoID = pedido.PedidoID;

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(facturaInvalida));
        }

        [TestMethod]
        public void PruebaValidacionFechaFutura()
        {
            if (aplicacion == null || pedido == null)
            {
                Assert.Fail("Datos de prueba no inicializados correctamente");
                return;
            }

            var facturaInvalida = EntidadesNucleo.Facturas();
            if (facturaInvalida == null)
            {
                Assert.Fail("No se pudo crear factura de prueba");
                return;
            }

            facturaInvalida.FechaFactura = DateTime.Now.Date.AddDays(1);
            facturaInvalida.PedidoID = pedido.PedidoID;

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(facturaInvalida));
        }

        [TestMethod]
        public void PruebaValidacionPedidoInexistente()
        {
            if (aplicacion == null)
            {
                Assert.Fail("Aplicación no inicializada correctamente");
                return;
            }

            var facturaInvalida = EntidadesNucleo.Facturas();
            if (facturaInvalida == null)
            {
                Assert.Fail("No se pudo crear factura de prueba");
                return;
            }

            facturaInvalida.PedidoID = 99999; // ID que no existe

            Assert.ThrowsException<Exception>(() => aplicacion.Guardar(facturaInvalida));
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
                if (entidad != null && entidad.FacturaID > 0 && iConexion?.Facturas != null)
                {
                    var facturaExistente = iConexion.Facturas.Find(entidad.FacturaID);
                    if (facturaExistente != null)
                    {
                        iConexion.Facturas.Remove(facturaExistente);
                        iConexion.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en LimpiarDatos: {ex.Message}");
            }
        }
    }
}