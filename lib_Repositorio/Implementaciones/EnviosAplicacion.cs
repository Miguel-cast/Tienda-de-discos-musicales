using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class EnviosAplicacion
    {
        private IConexion? IConexion = null;

        public EnviosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Envios? Borrar(Envios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.EnvioID == 0)
                throw new Exception("lbNoSeGuardo");

            // Lógica de negocio: No permitir borrar envíos ya despachados (fecha pasada)
            if (entidad.FechaEnvio < DateTime.Now.Date)
                throw new Exception("lbNoPuedeBorrarEnvioYaDespachado");

            this.IConexion!.Envios!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Envios? Guardar(Envios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.EnvioID != 0)
                throw new Exception("lbYaSeGuardo");

            // Lógica de negocio: Validar dirección no vacía
            if (string.IsNullOrWhiteSpace(entidad.DireccionEntrega))
                throw new Exception("lbDireccionEntregaRequerida");

            // Lógica de negocio: Validar ciudad no vacía
            if (string.IsNullOrWhiteSpace(entidad.CiudadEntrega))
                throw new Exception("lbCiudadEntregaRequerida");

            // Lógica de negocio: Validar país no vacío
            if (string.IsNullOrWhiteSpace(entidad.PaisEntrega))
                throw new Exception("lbPaisEntregaRequerido");

            // Lógica de negocio: Validar que la fecha de envío no sea anterior a hoy
            if (entidad.FechaEnvio < DateTime.Now.Date)
                throw new Exception("lbFechaEnvioNoDebeSerPasada");

            // Lógica de negocio: Verificar que el pedido existe
            var pedidoExiste = this.IConexion!.Pedidos!.Any(p => p.PedidoID == entidad.PedidoID);
            if (!pedidoExiste)
                throw new Exception("lbPedidoNoExiste");

            // Lógica de negocio: Verificar que el pedido no tenga ya un envío
            var yaExisteEnvio = this.IConexion!.Envios!.Any(e => e.PedidoID == entidad.PedidoID);
            if (yaExisteEnvio)
                throw new Exception("lbPedidoYaTieneEnvio");

            // Lógica de negocio: Verificar que el pedido esté en estado "Completado"
            var pedido = this.IConexion!.Pedidos!.Find(entidad.PedidoID);
            if (pedido?.Estado != "Completado")
                throw new Exception("lbPedidoDebeEstarCompletado");

            this.IConexion!.Envios!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Envios> Listar()
        {
            return this.IConexion!.Envios!
                .Include(e => e.Pedido)
                .Take(20)
                .ToList();
        }

        public Envios? Modificar(Envios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.EnvioID == 0)
                throw new Exception("lbNoSeGuardo");

            // Lógica de negocio: No permitir modificar envíos ya despachados
            if (entidad.FechaEnvio < DateTime.Now.Date)
                throw new Exception("lbNoPuedeModificarEnvioYaDespachado");

            // Aplicar validaciones básicas
            if (string.IsNullOrWhiteSpace(entidad.DireccionEntrega))
                throw new Exception("lbDireccionEntregaRequerida");

            if (string.IsNullOrWhiteSpace(entidad.CiudadEntrega))
                throw new Exception("lbCiudadEntregaRequerida");

            if (string.IsNullOrWhiteSpace(entidad.PaisEntrega))
                throw new Exception("lbPaisEntregaRequerido");

            var entry = this.IConexion!.Entry<Envios>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        // Métodos específicos de lógica de negocio
        public List<Envios> ObtenerEnviosPorCiudad(string ciudad)
        {
            return this.IConexion!.Envios!
                .Include(e => e.Pedido)
                .Where(e => e.CiudadEntrega!.ToLower().Contains(ciudad.ToLower()))
                .OrderBy(e => e.FechaEnvio)
                .ToList();
        }

        public List<Envios> ObtenerEnviosPorFecha(DateTime fechaInicio, DateTime fechaFin)
        {
            return this.IConexion!.Envios!
                .Include(e => e.Pedido)
                .Where(e => e.FechaEnvio >= fechaInicio && e.FechaEnvio <= fechaFin)
                .OrderByDescending(e => e.FechaEnvio)
                .ToList();
        }

        public List<Envios> ObtenerEnviosPendientes()
        {
            var fechaActual = DateTime.Now.Date;
            return this.IConexion!.Envios!
                .Include(e => e.Pedido)
                .Where(e => e.FechaEnvio >= fechaActual)
                .OrderBy(e => e.FechaEnvio)
                .ToList();
        }

        public List<Envios> ObtenerEnviosRealizados()
        {
            var fechaActual = DateTime.Now.Date;
            return this.IConexion!.Envios!
                .Include(e => e.Pedido)
                .Where(e => e.FechaEnvio < fechaActual)
                .OrderByDescending(e => e.FechaEnvio)
                .ToList();
        }

        public int ContarEnviosPorPais(string pais)
        {
            return this.IConexion!.Envios!
                .Count(e => e.PaisEntrega!.ToLower() == pais.ToLower());
        }

        public Envios? ObtenerEnvioPorPedido(int pedidoId)
        {
            return this.IConexion!.Envios!
                .Include(e => e.Pedido)
                .FirstOrDefault(e => e.PedidoID == pedidoId);
        }
    }
}