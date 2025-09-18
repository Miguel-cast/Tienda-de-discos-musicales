﻿using lib_dominio.Entidades;
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