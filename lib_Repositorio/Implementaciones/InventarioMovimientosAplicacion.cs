using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class InventarioMovimientosAplicacion
    {
        private IConexion? IConexion = null;

        public InventarioMovimientosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public InventarioMovimientos? Borrar(InventarioMovimientos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.MovimientoId == 0)
                throw new Exception("lbNoSeGuardo");
            this.IConexion!.InventarioMovimientos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public InventarioMovimientos? Guardar(InventarioMovimientos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.MovimientoId != 0)
                throw new Exception("lbYaSeGuardo");
            this.IConexion!.InventarioMovimientos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<InventarioMovimientos> Listar()
        {
            return this.IConexion!.InventarioMovimientos!.Take(20).ToList();
        }

        public InventarioMovimientos? Modificar(InventarioMovimientos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.MovimientoId == 0)
                throw new Exception("lbNoSeGuardo");
            var entry = this.IConexion!.Entry<InventarioMovimientos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}