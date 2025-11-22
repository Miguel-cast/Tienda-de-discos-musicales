using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class ProveedoresAplicacion : IProveedoresAplicacion
    {
        private IConexion? IConexion = null;
        private IAuditoriasAplicacion? IAuditoriasAplicacion = null;

        public ProveedoresAplicacion(IConexion iConexion, IAuditoriasAplicacion iAuditoriasAplicacion)
        {
            this.IConexion = iConexion;
            this.IAuditoriasAplicacion = iAuditoriasAplicacion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Proveedores? Borrar(Proveedores? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.ProveedoresId == 0)
                throw new Exception("lbNoSeGuardo");
            this.IConexion!.Proveedores!.Remove(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Proveedores",
                Accion = "Borrar",
                Descripcion = $"ProveedoresId={entidad.ProveedoresId}",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public Proveedores? Guardar(Proveedores? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.ProveedoresId != 0)
                throw new Exception("lbYaSeGuardo");
            this.IConexion!.Proveedores!.Add(entidad);
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Proveedores",
                Accion = "Guardar",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public List<Proveedores> Listar()
        {
            return this.IConexion!.Proveedores!.Take(20).ToList();
        }

        public Proveedores? Modificar(Proveedores? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.ProveedoresId == 0)
                throw new Exception("lbNoSeGuardo");
            var entry = this.IConexion!.Entry<Proveedores>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();

            this.IAuditoriasAplicacion!.Configurar(this.IConexion.StringConexion!);
            this.IAuditoriasAplicacion!.Guardar(new Auditorias
            {
                Usuario = "admin",
                Tabla = "Proveedores",
                Accion = "Modificar",
                Fecha = DateTime.Now
            });
            return entidad;
        }

        public List<Proveedores> PorNombreEmpresa(Proveedores? entidad)
        {
            return this.IConexion!.Proveedores!
                .Where(x => x.NombreEmpresa!.Contains(entidad!.NombreEmpresa!))
                .Take(50)
                .ToList();
        }
    }
}