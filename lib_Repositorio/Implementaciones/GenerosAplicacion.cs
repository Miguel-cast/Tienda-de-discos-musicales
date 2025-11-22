using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace lib_repositorios.Implementaciones
{
    public class GenerosAplicacion : IGenerosAplicacion
    {
        private IConexion? IConexion = null;
        private IAuditoriasAplicacion? IAuditoriasAplicacion = null;

        public GenerosAplicacion(IConexion iConexion, IAuditoriasAplicacion iAuditoriasAplicacion)
        {
            this.IConexion = iConexion;
            this.IAuditoriasAplicacion = iAuditoriasAplicacion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Generos? Borrar(Generos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.GenerosId == 0)
                throw new Exception("lbNoSeGuardo");
            this.IConexion!.Generos!.Remove(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Generos",
                Accion = "Borrar",
                Descripcion = $"GenerosId={entidad.GenerosId}",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public Generos? Guardar(Generos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.GenerosId != 0)
                throw new Exception("lbYaSeGuardo");
            this.IConexion!.Generos!.Add(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Generos",
                Accion = "Guardar",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public List<Generos> Listar()
        {
            return this.IConexion!.Generos!.Take(20).ToList();
        }

        public Generos? Modificar(Generos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.GenerosId == 0)
                throw new Exception("lbNoSeGuardo");
            var entry = this.IConexion!.Entry<Generos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Generos",
                Accion = "Modificar",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public List<Generos> PorNombreGenero(Generos? entidad)
        {
            return this.IConexion!.Generos!
                .Where(x => x.NombreGenero!.Contains(entidad!.NombreGenero!))
                .Take(50)
                .ToList();
        }
    }
}