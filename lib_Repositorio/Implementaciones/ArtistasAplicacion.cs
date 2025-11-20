using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_repositorios.Implementaciones
{
    public class ArtistasAplicacion : IArtistasAplicacion
    {
        private IConexion? IConexion = null;

        public ArtistasAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Artistas? Borrar(Artistas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.ArtistaId == 0)
                throw new Exception("lbNoSeGuardo");
            this.IConexion!.Artistas!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Artistas? Guardar(Artistas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.ArtistaId != 0)
                throw new Exception("lbYaSeGuardo");
            if (string.IsNullOrWhiteSpace(entidad.NombreArtista))
                throw new Exception("El nombre del artista no puede ser nulo o vacío");
            this.IConexion!.Artistas!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Artistas> Listar()
        {
            return this.IConexion!.Artistas!.Take(20).ToList();
        }

        public Artistas? Modificar(Artistas? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.ArtistaId == 0)
                throw new Exception("lbNoSeGuardo");
            var entry = this.IConexion!.Entry<Artistas>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Artistas> PorNombreArtista(Artistas? entidad)
        {
            return this.IConexion!.Artistas!
                .Where(x => x.NombreArtista!.Contains(entidad!.NombreArtista!))
                .Take(50)
                .ToList();
        }
    }
}
