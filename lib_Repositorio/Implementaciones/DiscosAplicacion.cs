using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_Repositorio.Implementaciones
{
    public class DiscosAplicacion
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
            if (entidad!.DiscoId== 0)
                throw new Exception("lbNoSeGuardo");
            // Operaciones
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
            // Operaciones
            this.IConexion!.Discos!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<Discos> Listar()
        {
            return this.IConexion!.Discos!.Take(20).ToList();
        }

        public Discos? Modificar(Discos? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad!.DiscoId == 0)
                throw new Exception(




                    "lbNoSeGuardo");
            // Operaciones
            var entry = this.IConexion!.Entry<Discos>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }
    }
}
