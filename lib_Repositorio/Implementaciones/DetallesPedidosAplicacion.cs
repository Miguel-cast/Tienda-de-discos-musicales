using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class DetallesPedidosAplicacion
    {
        private IConexion? IConexion = null;

        public DetallesPedidosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public DetallePedidos? Borrar(DetallePedidos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.DetallesId == 0)
                throw new Exception("lbNoSeGuardo");
            this.IConexion!.DetallePedidos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public DetallePedidos? Guardar(DetallePedidos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.DetallesId != 0)
                throw new Exception("lbYaSeGuardo");
            this.IConexion!.DetallePedidos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<DetallePedidos> Listar()
        {
            return this.IConexion!.DetallePedidos!.Take(20).ToList();
        }

        public DetallePedidos? Modificar(DetallePedidos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.DetallesId == 0)
                throw new Exception("lbNoSeGuardo");
            var entry = this.IConexion!.Entry<DetallePedidos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}