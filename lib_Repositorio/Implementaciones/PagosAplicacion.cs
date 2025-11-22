using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class PagosAplicacion : IPagosAplicacion
    {
        private IConexion? IConexion = null;
        private IAuditoriasAplicacion? IAuditoriasAplicacion = null;

        public PagosAplicacion(IConexion iConexion, IAuditoriasAplicacion iAuditoriasAplicacion)
        {
            this.IConexion = iConexion;
            this.IAuditoriasAplicacion = iAuditoriasAplicacion;
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

            var diasAntiguedad = (DateTime.Now.Date - entidad.FechaPago).Days;
            if (diasAntiguedad > 30)
                throw new Exception("lbNoPuedeBorrarPagoAntiguo");

            this.IConexion!.Pagos!.Remove(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Pagos",
                Accion = "Borrar",
                Descripcion = $"PagoID={entidad.PagoID}",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public Pagos? Guardar(Pagos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.PagoID != 0)
                throw new Exception("lbYaSeGuardo");

            if (entidad.Monto <= 0)
                throw new Exception("lbMontoDebeSerMayorACero");

            if (entidad.FechaPago > DateTime.Now.Date)
                throw new Exception("lbFechaPagoNoDebeSerFutura");

            var metodosValidos = new[] { "Efectivo", "Tarjeta de Crédito", "Tarjeta de Débito", "Transferencia", "Cheque" };
            if (string.IsNullOrWhiteSpace(entidad.MetodoPago) || !metodosValidos.Contains(entidad.MetodoPago))
                throw new Exception("lbMetodoPagoInvalido");

            var facturaExiste = this.IConexion!.Facturas!.Any(f => f.FacturaID == entidad.FacturaID);
            if (!facturaExiste)
                throw new Exception("lbFacturaNoExiste");

            var factura = this.IConexion!.Facturas!.Find(entidad.FacturaID);
            var totalPagado = this.IConexion!.Pagos!
                .Where(p => p.FacturaID == entidad.FacturaID)
                .Sum(p => p.Monto);

            var saldoPendiente = factura!.Total - totalPagado;
            if (entidad.Monto > saldoPendiente)
                throw new Exception("lbPagoExcedeSaldoPendiente");

            this.IConexion!.Pagos!.Add(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Pagos",
                Accion = "Guardar",
                Fecha = DateTime.Now
            });
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

            var diasAntiguedad = (DateTime.Now.Date - entidad.FechaPago).Days;
            if (diasAntiguedad > 7)
                throw new Exception("lbNoPuedeModificarPagoAntiguo");

            if (entidad.Monto <= 0)
                throw new Exception("lbMontoDebeSerMayorACero");

            var entry = this.IConexion!.Entry<Pagos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Pagos",
                Accion = "Modificar",
                Fecha = DateTime.Now
            });
            return entidad;
        }
    }
}