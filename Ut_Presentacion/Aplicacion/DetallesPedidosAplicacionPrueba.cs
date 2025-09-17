using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class DetallesPedidosAplicacionPrueba
    {
        private IConexion? iConexion;
        private DetallesPedidosAplicacion? aplicacion;
        private DetallePedidos? entidad;

        private Pedidos? pedido;
        private Discos? disco;
        private Artistas? artista;
        private Generos? genero;
        private Proveedores? proveedor;
        private Clientes? cliente;
        private Empleados? empleado;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new DetallesPedidosAplicacion(iConexion);

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

                // Crear pedido
                pedido = EntidadesNucleo.Pedidos();
                pedido.ClienteID = cliente!.ClienteId;
                pedido.EmpleadoID = empleado!.EmpleadoId;
                iConexion!.Pedidos!.Add(pedido);

                iConexion!.SaveChanges();
            }
            catch (Exception)
            {
                // Si ya existen, obtenerlos
                cliente = iConexion!.Clientes!.FirstOrDefault() ?? EntidadesNucleo.Clientes()!;
                empleado = iConexion!.Empleados!.FirstOrDefault() ?? EntidadesNucleo.Empleados()!;
                artista = iConexion!.Artistas!.FirstOrDefault() ?? EntidadesNucleo.Artistas()!;
                genero = iConexion!.Generos!.FirstOrDefault() ?? EntidadesNucleo.Generos()!;
                proveedor = iConexion!.Proveedores!.FirstOrDefault() ?? EntidadesNucleo.Proveedores()!;
                disco = iConexion!.Discos!.FirstOrDefault() ?? EntidadesNucleo.Discos()!;
                pedido = iConexion!.Pedidos!.FirstOrDefault() ?? EntidadesNucleo.Pedidos()!;
            }
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, PruebaGuardar());
            Assert.AreEqual(true, PruebaModificar());
            Assert.AreEqual(true, PruebaListar());
            Assert.AreEqual(true, PruebaObtenerPorPedido());
            Assert.AreEqual(true, PruebaCalcularTotalPorPedido());
            Assert.AreEqual(true, PruebaBorrar());
        }

        public bool PruebaGuardar()
        {
            try
            {
                entidad = EntidadesNucleo.DetallePedidos();
                entidad.PedidoId = pedido!.PedidoID;
                entidad.cantidad = 2;
                entidad.PrecioUnitario = 50000.00m;

                entidad = aplicacion!.Guardar(entidad);
                return entidad != null && entidad.DetallesId > 0;
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
                entidad!.cantidad = 3;
                entidad!.PrecioUnitario = 45000.00m;
                entidad = aplicacion!.Modificar(entidad);
                return entidad != null && entidad.cantidad == 3 && entidad.PrecioUnitario == 45000.00m;
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

        public bool PruebaObtenerPorPedido()
        {
            try
            {
                var detalles = aplicacion!.ObtenerPorPedido(pedido!.PedidoID);
                return detalles != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool PruebaCalcularTotalPorPedido()
        {
            try
            {
                var total = aplicacion!.CalcularTotalPorPedido(pedido!.PedidoID);
                return total >= 0;
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
        public void PruebaValidacionCantidadCero()
        {
            var detalleInvalido = EntidadesNucleo.DetallePedidos();
            detalleInvalido.PedidoId = pedido!.PedidoID;
            detalleInvalido.cantidad = 0;
            detalleInvalido.PrecioUnitario = 50000.00m;

            Assert.ThrowsException<Exception>(() => aplicacion!.Guardar(detalleInvalido));
        }

        [TestMethod]
        public void PruebaValidacionPrecioCero()
        {
            var detalleInvalido = EntidadesNucleo.DetallePedidos();
            detalleInvalido.PedidoId = pedido!.PedidoID;
            detalleInvalido.cantidad = 2;
            detalleInvalido.PrecioUnitario = 0;

            Assert.ThrowsException<Exception>(() => aplicacion!.Guardar(detalleInvalido));
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
                if (entidad != null && entidad.DetallesId > 0)
                {
                    var detalleExistente = iConexion!.DetallePedidos!.Find(entidad.DetallesId);
                    if (detalleExistente != null)
                    {
                        iConexion.DetallePedidos.Remove(detalleExistente);
                        iConexion.SaveChanges();
                    }
                }
            }
            catch { }
        }
    }
}
