using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class InventarioMovimientosAplicacionPruebas
    {
        private IConexion? iConexion;
        private InventarioMovimientosAplicacion? aplicacion;
        private InventarioMovimientos? entidad;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new InventarioMovimientosAplicacion(iConexion);
        }

        [TestMethod]
        public void PruebaGuardarMovimientoValido()
        {
            entidad = EntidadesNucleo.InventarioMovimientos();
            entidad.TipoMovimiento = "Entrada";
            entidad.Cantidad = 10;
            entidad.FechaMovimiento = DateTime.Now;
            var resultado = aplicacion!.Guardar(entidad);
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.MovimientoId > 0);
        }

        [TestMethod]
        public void PruebaValidacionCantidadCero()
        {
            entidad = EntidadesNucleo.InventarioMovimientos();
            entidad.TipoMovimiento = "Salida";
            entidad.Cantidad = 0;
            entidad.FechaMovimiento = DateTime.Now;
            var resultado = aplicacion!.Guardar(entidad);
            Assert.IsNotNull(resultado);
        }

        [TestCleanup]
        public void LimpiarDatos()
        {
            if (entidad != null && entidad.MovimientoId > 0)
            {
                var movimientoExistente = iConexion!.InventarioMovimientos!.Find(entidad.MovimientoId);
                if (movimientoExistente != null)
                {
                    iConexion.InventarioMovimientos.Remove(movimientoExistente);
                    iConexion.SaveChanges();
                }
            }
        }
    }
}

