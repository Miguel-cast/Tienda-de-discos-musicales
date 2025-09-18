using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class UsuariosSistemaAplicacion
    {
        private IConexion? IConexion = null;

        public UsuariosSistemaAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public UsuariosSistema? Borrar(UsuariosSistema? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.UsuarioId == 0)
                throw new Exception("lbNoSeGuardo");
            this.IConexion!.UsuariosSistema!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public UsuariosSistema? Guardar(UsuariosSistema? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.UsuarioId != 0)
                throw new Exception("lbYaSeGuardo");
            this.IConexion!.UsuariosSistema!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<UsuariosSistema> Listar()
        {
            return this.IConexion!.UsuariosSistema!.Take(20).ToList();
        }

        public UsuariosSistema? Modificar(UsuariosSistema? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.UsuarioId == 0)
                throw new Exception("lbNoSeGuardo");
            var entry = this.IConexion!.Entry<UsuariosSistema>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}