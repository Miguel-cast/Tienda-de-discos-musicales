using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class PedidosAplicacion
    {
        private IConexion? IConexion = null;

        public PedidosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Pedidos? Borrar(Pedidos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.PedidoID == 0)
                throw new Exception("lbNoSeGuardo");

            // Lógica de negocio: No permitir borrar pedidos con facturas asociadas
            var tieneFacturas = this.IConexion!.Facturas!.Any(f => f.PedidoID == entidad.PedidoID);
            if (tieneFacturas)
                throw new Exception("lbNoPuedeBorrarPedidoConFacturas");

            // Lógica de negocio: No permitir borrar pedidos con envíos asociados
            var tieneEnvios = this.IConexion!.Envios!.Any(e => e.PedidoID == entidad.PedidoID);
            if (tieneEnvios)
                throw new Exception("lbNoPuedeBorrarPedidoConEnvios");

            // Lógica de negocio: No permitir borrar pedidos con detalles asociados
            var tieneDetalles = this.IConexion!.DetallePedidos!.Any(d => d.PedidoId == entidad.PedidoID);
            if (tieneDetalles)
                throw new Exception("lbNoPuedeBorrarPedidoConDetalles");

            this.IConexion!.Pedidos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Pedidos? Guardar(Pedidos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.PedidoID != 0)
                throw new Exception("lbYaSeGuardo");

            // Lógica de negocio: Validar que la fecha no sea futura
            if (entidad.FechaPedido > DateTime.Now.Date)
                throw new Exception("lbFechaPedidoNoDebeSerFutura");

            // Lógica de negocio: Validar estado válido
            var estadosValidos = new[] { "Pendiente", "En Proceso", "Completado", "Cancelado" };
            if (string.IsNullOrWhiteSpace(entidad.Estado) || !estadosValidos.Contains(entidad.Estado))
                throw new Exception("lbEstadoPedidoInvalido");

            // Lógica de negocio: Validar que el cliente existe
            var clienteExiste = this.IConexion!.Clientes!.Any(c => c.ClienteId == entidad.ClienteID);
            if (!clienteExiste)
                throw new Exception("lbClienteNoExiste");

            // Lógica de negocio: Validar que el empleado existe
            var empleadoExiste = this.IConexion!.Empleados!.Any(e => e.EmpleadoId == entidad.EmpleadoID);
            if (!empleadoExiste)
                throw new Exception("lbEmpleadoNoExiste");

            this.IConexion!.Pedidos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Pedidos> Listar()
        {
            return this.IConexion!.Pedidos!
                .Include(p => p.Cliente)
                .Include(p => p.Empleado)
                .Take(20)
                .ToList();
        }

        public Pedidos? Modificar(Pedidos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.PedidoID == 0)
                throw new Exception("lbNoSeGuardo");

            // Aplicar las mismas validaciones que en Guardar
            if (entidad.FechaPedido > DateTime.Now.Date)
                throw new Exception("lbFechaPedidoNoDebeSerFutura");

            var estadosValidos = new[] { "Pendiente", "En Proceso", "Completado", "Cancelado" };
            if (string.IsNullOrWhiteSpace(entidad.Estado) || !estadosValidos.Contains(entidad.Estado))
                throw new Exception("lbEstadoPedidoInvalido");

            var entry = this.IConexion!.Entry<Pedidos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        // Métodos específicos de lógica de negocio
        public List<Pedidos> ObtenerPedidosPorCliente(int clienteId)
        {
            return this.IConexion!.Pedidos!
                .Include(p => p.Cliente)
                .Where(p => p.ClienteID == clienteId)
                .OrderByDescending(p => p.FechaPedido)
                .ToList();
        }

        public List<Pedidos> ObtenerPedidosPorEstado(string estado)
        {
            return this.IConexion!.Pedidos!
                .Include(p => p.Cliente)
                .Where(p => p.Estado == estado)
                .ToList();
        }

        public int ContarPedidosPorMes(int año, int mes)
        {
            return this.IConexion!.Pedidos!
                .Count(p => p.FechaPedido.Year == año && p.FechaPedido.Month == mes);
        }

        public List<Pedidos> ObtenerPedidosRecientes(int dias = 7)
        {
            var fechaMinima = DateTime.Now.Date.AddDays(-dias);
            return this.IConexion!.Pedidos!
                .Include(p => p.Cliente)
                .Where(p => p.FechaPedido >= fechaMinima)
                .OrderByDescending(p => p.FechaPedido)
                .ToList();
        }
    }
}