using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class PagosAplicacion
    {
        private IConexion? IConexion = null;

        public PagosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Pagos? Borrar(Pagos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.PagoID == 0)
                throw new Exception("lbNoSeGuardo");

            // Lógica de negocio: Validar que el pago no sea muy antiguo (ej: más de 30 días)
            var diasAntiguedad = (DateTime.Now.Date - entidad.FechaPago).Days;
            if (diasAntiguedad > 30)
                throw new Exception("lbNoPuedeBorrarPagoAntiguo");

            this.IConexion!.Pagos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Pagos? Guardar(Pagos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.PagoID != 0)
                throw new Exception("lbYaSeGuardo");

            // Lógica de negocio: Validar que el monto sea mayor a 0
            if (entidad.Monto <= 0)
                throw new Exception("lbMontoDebeSerMayorACero");

            // Lógica de negocio: Validar que la fecha no sea futura
            if (entidad.FechaPago > DateTime.Now.Date)
                throw new Exception("lbFechaPagoNoDebeSerFutura");

            // Lógica de negocio: Validar método de pago válido
            var metodosValidos = new[] { "Efectivo", "Tarjeta de Crédito", "Tarjeta de Débito", "Transferencia", "Cheque" };
            if (string.IsNullOrWhiteSpace(entidad.MetodoPago) || !metodosValidos.Contains(entidad.MetodoPago))
                throw new Exception("lbMetodoPagoInvalido");

            // Lógica de negocio: Verificar que la factura existe
            var facturaExiste = this.IConexion!.Facturas!.Any(f => f.FacturaID == entidad.FacturaID);
            if (!facturaExiste)
                throw new Exception("lbFacturaNoExiste");

            // Lógica de negocio: Verificar que el pago no exceda el saldo pendiente
            var factura = this.IConexion!.Facturas!.Find(entidad.FacturaID);
            var totalPagado = this.IConexion!.Pagos!
                .Where(p => p.FacturaID == entidad.FacturaID)
                .Sum(p => p.Monto);

            var saldoPendiente = factura!.Total - totalPagado;
            if (entidad.Monto > saldoPendiente)
                throw new Exception("lbPagoExcedeSaldoPendiente");

            this.IConexion!.Pagos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Pagos> Listar()
        {
            return this.IConexion!.Pagos!
                .Include(p => p.Factura)
                .Take(20)
                .ToList();
        }

        public Pagos? Modificar(Pagos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.PagoID == 0)
                throw new Exception("lbNoSeGuardo");

            // Lógica de negocio: No permitir modificar pagos antiguos
            var diasAntiguedad = (DateTime.Now.Date - entidad.FechaPago).Days;
            if (diasAntiguedad > 7)
                throw new Exception("lbNoPuedeModificarPagoAntiguo");

            // Aplicar validaciones básicas
            if (entidad.Monto <= 0)
                throw new Exception("lbMontoDebeSerMayorACero");

            var entry = this.IConexion!.Entry<Pagos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        // Métodos específicos de lógica de negocio
        public List<Pagos> ObtenerPagosPorFactura(int facturaId)
        {
            return this.IConexion!.Pagos!
                .Include(p => p.Factura)
                .Where(p => p.FacturaID == facturaId)
                .OrderBy(p => p.FechaPago)
                .ToList();
        }

        public decimal CalcularTotalPagosPorMetodo(string metodoPago)
        {
            return this.IConexion!.Pagos!
                .Where(p => p.MetodoPago == metodoPago)
                .Sum(p => p.Monto);
        }

        public List<Pagos> ObtenerPagosPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            return this.IConexion!.Pagos!
                .Include(p => p.Factura)
                .Where(p => p.FechaPago >= fechaInicio && p.FechaPago <= fechaFin)
                .OrderByDescending(p => p.FechaPago)
                .ToList();
        }

        public decimal CalcularSaldoPendienteFactura(int facturaId)
        {
            var factura = this.IConexion!.Facturas!.Find(facturaId);
            if (factura == null) return 0;

            var totalPagado = this.IConexion!.Pagos!
                .Where(p => p.FacturaID == facturaId)
                .Sum(p => p.Monto);

            return factura.Total - totalPagado;
        }

        public List<Pagos> ObtenerPagosRecientes(int dias = 7)
        {
            var fechaMinima = DateTime.Now.Date.AddDays(-dias);
            return this.IConexion!.Pagos!
                .Include(p => p.Factura)
                .Where(p => p.FechaPago >= fechaMinima)
                .OrderByDescending(p => p.FechaPago)
                .ToList();
        }
    }
}