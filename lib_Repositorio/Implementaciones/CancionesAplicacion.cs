using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class CancionesAplicacion
    {
        private IConexion? IConexion = null;

        public CancionesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Canciones? Borrar(Canciones? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.CancionId == 0)
                throw new Exception("lbNoSeGuardo");
            this.IConexion!.Canciones!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Canciones? Guardar(Canciones? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.CancionId != 0)
                throw new Exception("lbYaSeGuardo");
            this.IConexion!.Canciones!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Canciones> Listar()
        {
            return this.IConexion!.Canciones!.Take(20).ToList();
        }

        public Canciones? Modificar(Canciones? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.CancionId == 0)
                throw new Exception("lbNoSeGuardo");
            var entry = this.IConexion!.Entry<Canciones>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}
