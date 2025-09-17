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
                throw new Exception("Falta información");
            if (entidad.CancionId != 0)
                throw new Exception("La canción ya fue guardada");

            // Validaciones de negocio
            if (string.IsNullOrWhiteSpace(entidad.Titulo))
                throw new Exception("El título de la canción es obligatorio.");

            if (entidad.DiscoID <= 0)
                throw new Exception("Debe asociar la canción a un disco válido.");

            if (!this.IConexion!.Discos!.Any(d => d.DiscoId == entidad.DiscoID))
                throw new Exception("El disco asociado no existe.");



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

        // 🔎 Métodos específicos de negocio

        public List<Canciones> ObtenerPorDisco(int discoId)
        {
            return this.IConexion!.Canciones!
                .Where(c => c.DiscoID == discoId)
                .ToList();
        }

        public List<Canciones> BuscarPorTitulo(string titulo)
        {
            return this.IConexion!.Canciones!
                .Where(c => c.Titulo.Contains(titulo))
                .ToList();
        }

    }
}
