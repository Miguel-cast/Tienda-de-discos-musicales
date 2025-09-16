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

        public DetallesPedidos? Borrar(DetallesPedidos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.DetallesId == 0)
                throw new Exception("lbNoSeGuardo");
            this.IConexion!.DetallesPedidos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public DetallesPedidos? Guardar(DetallesPedidos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.DetallesId != 0)
                throw new Exception("lbYaSeGuardo");
            this.IConexion!.DetallesPedidos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<DetallesPedidos> Listar()
        {
            return this.IConexion!.DetallesPedidos!.Take(20).ToList();
        }

        public DetallesPedidos? Modificar(DetallesPedidos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.DetallesId == 0)
                throw new Exception("lbNoSeGuardo");
            var entry = this.IConexion!.Entry<DetallesPedidos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}