using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class GenerosAplicacion : IGenerosAplicacion
    {
        private IConexion? IConexion = null;

        public GenerosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
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
            return entidad;
        }
    }
}