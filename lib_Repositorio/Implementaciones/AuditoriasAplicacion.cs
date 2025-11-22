using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class AuditoriasAplicacion : IAuditoriasAplicacion
    {
        private IConexion? IConexion = null;

        public AuditoriasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Auditorias? Borrar(Auditorias? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.AuditoriaId == 0)
                throw new Exception("lbNoSeGuardo");

            this.IConexion!.Auditorias!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Auditorias? Guardar(Auditorias? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad.AuditoriaId != 0)
                throw new Exception("lbYaSeGuardo");

            //// Asegurar fecha y truncar/normalizar campos si hace falta
            //if (entidad.FechaHora == DateTime.MinValue)
            //    entidad.FechaHora = DateTime.Now;

            //if (!string.IsNullOrWhiteSpace(entidad.Accion))
            //    entidad.Accion = entidad.Accion!.Trim();

            //if (!string.IsNullOrWhiteSpace(entidad.Tabla))
            //    entidad.Tabla = entidad.Tabla!.Trim();

            //if (!string.IsNullOrWhiteSpace(entidad.Descripcion))
            //    entidad.Descripcion = entidad.Descripcion!.Trim();

            this.IConexion!.Auditorias!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Auditorias> Listar()
        {
            return this.IConexion!.Auditorias!.OrderByDescending(a => a.Fecha).Take(50).ToList();
        }

        public List<Auditorias> PorUsuario(Auditorias? entidad)
        {
            if (entidad == null)
                return new List<Auditorias>();

             return this.IConexion!.Auditorias!
                .Where(x => x.Usuario!.Contains(entidad!.Usuario!))
                .ToList();
        }

        public Auditorias? Modificar(Auditorias? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");

            if (entidad!.AuditoriaId == 0)
                throw new Exception("lbNoSeGuardo");

            var entry = this.IConexion!.Entry<Auditorias>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}