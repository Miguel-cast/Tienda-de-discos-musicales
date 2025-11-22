using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class InventarioMovimientosAplicacion : IInventarioMovimientosAplicacion
    {
        private IConexion? IConexion = null;
        private IAuditoriasAplicacion? IAuditoriasAplicacion = null;

        public InventarioMovimientosAplicacion(IConexion iConexion, IAuditoriasAplicacion iAuditoriasAplicacion)
        {
            this.IConexion = iConexion;
            this.IAuditoriasAplicacion = iAuditoriasAplicacion;
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

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "InventarioMovimientos",
                Accion = "Borrar",
                Descripcion = $"MovimientoId={entidad.MovimientoId}",
                Fecha = DateTime.Now
            });
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

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "InventarioMovimientos",
                Accion = "Guardar",
                Fecha = DateTime.Now
            });
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

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "InventarioMovimientos",
                Accion = "Modificar",
                Fecha = DateTime.Now
            });
            return entidad;
        }
    }
}