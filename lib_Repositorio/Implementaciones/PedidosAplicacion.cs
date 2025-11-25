using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class PedidosAplicacion : IPedidosAplicacion
    {
        private IConexion? IConexion = null;
        private IAuditoriasAplicacion? IAuditoriasAplicacion = null;

        public PedidosAplicacion(IConexion iConexion, IAuditoriasAplicacion iAuditoriasAplicacion)
        {
            this.IConexion = iConexion;
            this.IAuditoriasAplicacion = iAuditoriasAplicacion;
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

            var tieneFacturas = this.IConexion!.Facturas!.Any(f => f.PedidoID == entidad.PedidoID);
            if (tieneFacturas)
                throw new Exception("lbNoPuedeBorrarPedidoConFacturas");

            var tieneEnvios = this.IConexion!.Envios!.Any(e => e.PedidoID == entidad.PedidoID);
            if (tieneEnvios)
                throw new Exception("lbNoPuedeBorrarPedidoConEnvios");

            var tieneDetalles = this.IConexion!.DetallePedidos!.Any(d => d.PedidoId == entidad.PedidoID);
            if (tieneDetalles)
                throw new Exception("lbNoPuedeBorrarPedidoConDetalles");

            this.IConexion!.Pedidos!.Remove(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Pedidos",
                Accion = "Borrar",
                Descripcion = $"PedidoID={entidad.PedidoID}",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public Pedidos? Guardar(Pedidos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.PedidoID != 0)
                throw new Exception("lbYaSeGuardo");

            if (entidad.FechaPedido > DateTime.Now.Date)
                throw new Exception("lbFechaPedidoNoDebeSerFutura");

            var estadosValidos = new[] { "Pendiente", "En Proceso", "Completado", "Cancelado" };
            if (string.IsNullOrWhiteSpace(entidad.Estado) || !estadosValidos.Contains(entidad.Estado))
                throw new Exception("lbEstadoPedidoInvalido");

            var clienteExiste = this.IConexion!.Clientes!.Any(c => c.ClienteId == entidad.ClienteID);
            if (!clienteExiste)
                throw new Exception("lbClienteNoExiste");

            var empleadoExiste = this.IConexion!.Empleados!.Any(e => e.EmpleadoId == entidad.EmpleadoID);
            if (!empleadoExiste)
                throw new Exception("lbEmpleadoNoExiste");

            this.IConexion!.Pedidos!.Add(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Pedidos",
                Accion = "Guardar",
                Fecha = DateTime.Now
            });
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

            if (entidad.FechaPedido > DateTime.Now.Date)
                throw new Exception("lbFechaPedidoNoDebeSerFutura");

            var estadosValidos = new[] { "Pendiente", "En Proceso", "Completado", "Cancelado" };
            if (string.IsNullOrWhiteSpace(entidad.Estado) || !estadosValidos.Contains(entidad.Estado))
                throw new Exception("lbEstadoPedidoInvalido");

            var entry = this.IConexion!.Entry<Pedidos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Pedidos",
                Accion = "Modificar",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public List<Pedidos> ObtenerPedidosPorCliente(int clienteId)
        {
            return this.IConexion!.Pedidos!
                .Include(p => p.Cliente)
                .Where(p => p.ClienteID == clienteId)
                .OrderByDescending(p => p.FechaPedido)
                .ToList();
        }

        public List<Pedidos> PorEstado(Pedidos? entidad)
        {
            return this.IConexion!.Pedidos!
                .Include(d => d.Cliente)
                .Include(d => d.Empleado)
                .Where(x => x.Estado!.Contains(entidad!.Estado!))
                .Take(50)
                .ToList();
        }

    }
}