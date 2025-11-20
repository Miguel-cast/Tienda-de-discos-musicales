using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class DiscosAplicacion : IDiscosAplicacion
    {
        private IConexion? IConexion = null;

        public DiscosAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public Discos? Borrar(Discos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.DiscoId == 0)
                throw new Exception("lbNoSeGuardo");

            var tieneCanciones = this.IConexion!.Canciones!.Any(c => c.DiscoID == entidad.DiscoId);
            if (tieneCanciones)
                throw new Exception("lbNoPuedeBorrarDiscoConCanciones");

            var tieneMovimientos = this.IConexion!.InventarioMovimientos!.Any(m => m.DiscoId == entidad.DiscoId);
            if (tieneMovimientos)
                throw new Exception("lbNoPuedeBorrarDiscoConMovimientos");

            var tieneReseñas = this.IConexion!.ReseñasClientes!.Any(r => r.DiscoID == entidad.DiscoId);
            if (tieneReseñas)
                throw new Exception("lbNoPuedeBorrarDiscoConReseñas");

            this.IConexion!.Discos!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public Discos? Guardar(Discos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.DiscoId != 0)
                throw new Exception("lbYaSeGuardo");

            if (entidad.Precio <= 0)
                throw new Exception("lbPrecioDebeSerMayorACero");

            if (entidad.AñoLanzamiento < 1900 || entidad.AñoLanzamiento > DateTime.Now.Year + 1)
                throw new Exception("lbAñoLanzamientoInvalido");

            var artistaExiste = this.IConexion!.Artistas!.Any(a => a.ArtistaId == entidad.ArtistaId);
            if (!artistaExiste)
                throw new Exception("lbArtistaNoExiste");

            var generoExiste = this.IConexion!.Generos!.Any(g => g.GenerosId == entidad.GenerosId);
            if (!generoExiste)
                throw new Exception("lbGeneroNoExiste");

            var proveedorExiste = this.IConexion!.Proveedores!.Any(p => p.ProveedoresId == entidad.ProveedoresId);
            if (!proveedorExiste)
                throw new Exception("lbProveedorNoExiste");

            if (string.IsNullOrWhiteSpace(entidad.Titulo))
                throw new Exception("lbTituloRequerido");

            this.IConexion!.Discos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Discos> Listar()
        {
            return this.IConexion!.Discos!
                .Include(d => d.Artista)
                .Include(d => d.Genero)
                .Include(d => d.Proveedor)
                .Take(20)
                .ToList();
        }

        public Discos? Modificar(Discos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.DiscoId == 0)
                throw new Exception("lbNoSeGuardo");

            if (entidad.Precio <= 0)
                throw new Exception("lbPrecioDebeSerMayorACero");

            if (entidad.AñoLanzamiento < 1900 || entidad.AñoLanzamiento > DateTime.Now.Year + 1)
                throw new Exception("lbAñoLanzamientoInvalido");

            if (string.IsNullOrWhiteSpace(entidad.Titulo))
                throw new Exception("lbTituloRequerido");

            var entry = this.IConexion!.Entry<Discos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Discos> ObtenerDiscosPorArtista(int artistaId)
        {
            return this.IConexion!.Discos!
                .Include(d => d.Artista)
                .Where(d => d.ArtistaId == artistaId)
                .ToList();
        }

        public List<Discos> ObtenerDiscosPorGenero(int generoId)
        {
            return this.IConexion!.Discos!
                .Include(d => d.Genero)
                .Where(d => d.GenerosId == generoId)
                .ToList();
        }

        public decimal CalcularPromedioPrecios()
        {
            var discos = this.IConexion!.Discos!.ToList();
            return discos.Any() ? discos.Average(d => d.Precio) : 0;
        }

        public List<Discos> ObtenerDiscosRecientes(int añosAtras = 5)
        {
            var añoMinimo = DateTime.Now.Year - añosAtras;
            return this.IConexion!.Discos!
                .Where(d => d.AñoLanzamiento >= añoMinimo)
                .OrderByDescending(d => d.AñoLanzamiento)
                .ToList();
        }

        public List<Discos> BuscarPorTitulo(string titulo)
        {
            return this.IConexion!.Discos!
                .Include(d => d.Artista)
                .Where(d => d.Titulo!.Contains(titulo))
                .ToList();
        }
    }
}