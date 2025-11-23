using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class EnviosAplicacion : IEnviosAplicacion
    {
        private IConexion? IConexion = null;
        private IAuditoriasAplicacion? IAuditoriasAplicacion = null;

        public EnviosAplicacion(IConexion iConexion, IAuditoriasAplicacion iAuditoriasAplicacion)
        {
            this.IConexion = iConexion;
            this.IAuditoriasAplicacion = iAuditoriasAplicacion;
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

            if (entidad.FechaEnvio < DateTime.Now.Date)
                throw new Exception("lbNoPuedeBorrarEnvioYaDespachado");

            this.IConexion!.Envios!.Remove(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Envios",
                Accion = "Borrar",
                Descripcion = $"EnvioID={entidad.EnvioID}",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public Envios? Guardar(Envios? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.EnvioID != 0)
                throw new Exception("lbYaSeGuardo");

            if (string.IsNullOrWhiteSpace(entidad.DireccionEntrega))
                throw new Exception("lbDireccionEntregaRequerida");

            if (string.IsNullOrWhiteSpace(entidad.CiudadEntrega))
                throw new Exception("lbCiudadEntregaRequerida");

            if (string.IsNullOrWhiteSpace(entidad.PaisEntrega))
                throw new Exception("lbPaisEntregaRequerido");

            if (entidad.FechaEnvio < DateTime.Now.Date)
                throw new Exception("lbFechaEnvioNoDebeSerPasada");

            var pedidoExiste = this.IConexion!.Pedidos!.Any(p => p.PedidoID == entidad.PedidoID);
            if (!pedidoExiste)
                throw new Exception("lbPedidoNoExiste");

            var yaExisteEnvio = this.IConexion!.Envios!.Any(e => e.PedidoID == entidad.PedidoID);
            if (yaExisteEnvio)
                throw new Exception("lbPedidoYaTieneEnvio");

            var pedido = this.IConexion!.Pedidos!.Find(entidad.PedidoID);
            if (pedido?.Estado != "Completado")
                throw new Exception("lbPedidoDebeEstarCompletado");

            this.IConexion!.Envios!.Add(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Envios",
                Accion = "Guardar",
                Fecha = DateTime.Now
            });
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

            if (entidad.FechaEnvio < DateTime.Now.Date)
                throw new Exception("lbNoPuedeModificarEnvioYaDespachado");

            if (string.IsNullOrWhiteSpace(entidad.DireccionEntrega))
                throw new Exception("lbDireccionEntregaRequerida");

            if (string.IsNullOrWhiteSpace(entidad.CiudadEntrega))
                throw new Exception("lbCiudadEntregaRequerida");

            if (string.IsNullOrWhiteSpace(entidad.PaisEntrega))
                throw new Exception("lbPaisEntregaRequerido");

            var entry = this.IConexion!.Entry<Envios>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Envios",
                Accion = "Modificar",
                Fecha = DateTime.Now
            });
            return entidad;
        }

    }
}