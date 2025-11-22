using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class FacturasAplicacion : IFacturasAplicacion
    {
        private IConexion? IConexion = null;
        private IAuditoriasAplicacion? IAuditoriasAplicacion = null;

        public FacturasAplicacion(IConexion iConexion, IAuditoriasAplicacion iAuditoriasAplicacion)
        {
            this.IConexion = iConexion;
            this.IAuditoriasAplicacion = iAuditoriasAplicacion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Facturas? Borrar(Facturas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.FacturaID == 0)
                throw new Exception("lbNoSeGuardo");

            var tienePagos = this.IConexion!.Pagos!.Any(p => p.FacturaID == entidad.FacturaID);
            if (tienePagos)
                throw new Exception("lbNoPuedeBorrarFacturaConPagos");

            this.IConexion!.Facturas!.Remove(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Facturas",
                Accion = "Borrar",
                Descripcion = $"FacturaID={entidad.FacturaID}",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public Facturas? Guardar(Facturas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.FacturaID != 0)
                throw new Exception("lbYaSeGuardo");

            if (entidad.Total <= 0)
                throw new Exception("lbTotalDebeSerMayorACero");

            if (entidad.FechaFactura > DateTime.Now.Date)
                throw new Exception("lbFechaFacturaNoDebeSerFutura");

            var pedidoExiste = this.IConexion!.Pedidos!.Any(p => p.PedidoID == entidad.PedidoID);
            if (!pedidoExiste)
                throw new Exception("lbPedidoNoExiste");

            var yaExisteFactura = this.IConexion!.Facturas!.Any(f => f.PedidoID == entidad.PedidoID);
            if (yaExisteFactura)
                throw new Exception("lbPedidoYaTieneFactura");

            this.IConexion!.Facturas!.Add(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Facturas",
                Accion = "Guardar",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public List<Facturas> Listar()
        {
            return this.IConexion!.Facturas!
                .Include(f => f.Pedido)
                .Include(f => f.Pagos)
                .Take(20)
                .ToList();
        }

        public Facturas? Modificar(Facturas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.FacturaID == 0)
                throw new Exception("lbNoSeGuardo");

            if (entidad.Total <= 0)
                throw new Exception("lbTotalDebeSerMayorACero");

            var totalPagos = this.IConexion!.Pagos!
                .Where(p => p.FacturaID == entidad.FacturaID)
                .Sum(p => p.Monto);

            if (totalPagos >= entidad.Total)
                throw new Exception("lbNoPuedeModificarFacturaPagada");

            var entry = this.IConexion!.Entry<Facturas>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Facturas",
                Accion = "Modificar",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public decimal CalcularTotalFacturasCliente(int clienteId)
        {
            return this.IConexion!.Facturas!
                .Include(f => f.Pedido)
                .Where(f => f.Pedido!.ClienteID == clienteId)
                .Sum(f => f.Total);
        }

        public List<Facturas> ObtenerFacturasPendientesPago()
        {
            return this.IConexion!.Facturas!
                .Include(f => f.Pagos)
                .Where(f => !f.Pagos!.Any() || f.Pagos!.Sum(p => p.Monto) < f.Total)
                .ToList();
        }

        public List<Facturas> ObtenerFacturasPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            return this.IConexion!.Facturas!
                .Include(f => f.Pedido)
                .Where(f => f.FechaFactura >= fechaInicio && f.FechaFactura <= fechaFin)
                .OrderByDescending(f => f.FechaFactura)
                .ToList();
        }

        public decimal CalcularMontoTotalMes(int año, int mes)
        {
            return this.IConexion!.Facturas!
                .Where(f => f.FechaFactura.Year == año && f.FechaFactura.Month == mes)
                .Sum(f => f.Total);
        }

        public Facturas? ObtenerFacturaPorPedido(int pedidoId)
        {
            return this.IConexion!.Facturas!
                .Include(f => f.Pedido)
                .Include(f => f.Pagos)
                .FirstOrDefault(f => f.PedidoID == pedidoId);
        }
    }
}