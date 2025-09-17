using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Implementaciones
{
    public class ReseñasClientesAplicacion
    {
        private IConexion? IConexion = null;

        public ReseñasClientesAplicacion(IConexion iConexion)
        {
            this.IConexion = iConexion;
        }

        public void Configurar(string StringConexion)
        {
            this.IConexion!.StringConexion = StringConexion;
        }

        public ReseñasClientes? Borrar(ReseñasClientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.ReseñaID == 0)
                throw new Exception("lbNoSeGuardo");

            // Lógica de negocio: Solo permitir borrar reseñas del mismo cliente
            // (En un escenario real, se validaría con el usuario actual)
            // Por ahora solo validamos que exista

            this.IConexion!.ReseñasClientes!.Remove(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public ReseñasClientes? Guardar(ReseñasClientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.ReseñaID != 0)
                throw new Exception("lbYaSeGuardo");

            // Lógica de negocio: Validar calificación entre 1 y 5
            if (entidad.Calificacion < 1 || entidad.Calificacion > 5)
                throw new Exception("lbCalificacionDebeEstarEntre1Y5");

            // Lógica de negocio: Validar comentario no vacío
            if (string.IsNullOrWhiteSpace(entidad.Comentario))
                throw new Exception("lbComentarioRequerido");

            // Lógica de negocio: Validar longitud mínima del comentario
            if (entidad.Comentario.Length < 10)
                throw new Exception("lbComentarioMuyCorto");

            // Lógica de negocio: Validar que la fecha no sea futura
            if (entidad.Fecha > DateTime.Now.Date)
                throw new Exception("lbFechaReseñaNoDebeSerFutura");

            // Lógica de negocio: Verificar que el cliente existe
            var clienteExiste = this.IConexion!.Clientes!.Any(c => c.ClienteId == entidad.ClienteID);
            if (!clienteExiste)
                throw new Exception("lbClienteNoExiste");

            // Lógica de negocio: Verificar que el disco existe
            var discoExiste = this.IConexion!.Discos!.Any(d => d.DiscoId == entidad.DiscoID);
            if (!discoExiste)
                throw new Exception("lbDiscoNoExiste");

            // Lógica de negocio: Un cliente solo puede hacer una reseña por disco
            var yaExisteReseña = this.IConexion!.ReseñasClientes!
                .Any(r => r.ClienteID == entidad.ClienteID && r.DiscoID == entidad.DiscoID);
            if (yaExisteReseña)
                throw new Exception("lbClienteYaReseñoEsteDisco");

            this.IConexion!.ReseñasClientes!.Add(entidad);
            this.IConexion.SaveChanges();
            return entidad;
        }

        public List<ReseñasClientes> Listar()
        {
            return this.IConexion!.ReseñasClientes!
                .Include(r => r.Cliente)
                .Include(r => r.Disco)
                .Take(20)
                .ToList();
        }

        public ReseñasClientes? Modificar(ReseñasClientes? entidad)
        {
            if (entidad == null)
                throw new Exception("lbFaltaInformacion");
            if (entidad.ReseñaID == 0)
                throw new Exception("lbNoSeGuardo");

            // Lógica de negocio: No permitir modificar reseñas muy antiguas (más de 30 días)
            var diasAntiguedad = (DateTime.Now.Date - entidad.Fecha).Days;
            if (diasAntiguedad > 30)
                throw new Exception("lbNoPuedeModificarReseñaAntigua");

            // Aplicar validaciones básicas
            if (entidad.Calificacion < 1 || entidad.Calificacion > 5)
                throw new Exception("lbCalificacionDebeEstarEntre1Y5");

            if (string.IsNullOrWhiteSpace(entidad.Comentario))
                throw new Exception("lbComentarioRequerido");

            if (entidad.Comentario.Length < 10)
                throw new Exception("lbComentarioMuyCorto");

            var entry = this.IConexion!.Entry<ReseñasClientes>(entidad);
            entry.State = EntityState.Modified;
            this.IConexion.SaveChanges();
            return entidad;
        }

        // Métodos específicos de lógica de negocio
        public List<ReseñasClientes> ObtenerReseñasPorDisco(int discoId)
        {
            return this.IConexion!.ReseñasClientes!
                .Include(r => r.Cliente)
                .Include(r => r.Disco)
                .Where(r => r.DiscoID == discoId)
                .OrderByDescending(r => r.Fecha)
                .ToList();
        }

        public List<ReseñasClientes> ObtenerReseñasPorCliente(int clienteId)
        {
            return this.IConexion!.ReseñasClientes!
                .Include(r => r.Cliente)
                .Include(r => r.Disco)
                .Where(r => r.ClienteID == clienteId)
                .OrderByDescending(r => r.Fecha)
                .ToList();
        }

        public double CalcularPromedioCalificacionDisco(int discoId)
        {
            var reseñas = this.IConexion!.ReseñasClientes!
                .Where(r => r.DiscoID == discoId)
                .ToList();

            return reseñas.Any() ? reseñas.Average(r => r.Calificacion) : 0;
        }

        public List<ReseñasClientes> ObtenerReseñasPorCalificacion(int calificacion)
        {
            return this.IConexion!.ReseñasClientes!
                .Include(r => r.Cliente)
                .Include(r => r.Disco)
                .Where(r => r.Calificacion == calificacion)
                .OrderByDescending(r => r.Fecha)
                .ToList();
        }

        public List<ReseñasClientes> ObtenerReseñasRecientes(int dias = 7)
        {
            var fechaMinima = DateTime.Now.Date.AddDays(-dias);
            return this.IConexion!.ReseñasClientes!
                .Include(r => r.Cliente)
                .Include(r => r.Disco)
                .Where(r => r.Fecha >= fechaMinima)
                .OrderByDescending(r => r.Fecha)
                .ToList();
        }

        public int ContarReseñasPorDisco(int discoId)
        {
            return this.IConexion!.ReseñasClientes!
                .Count(r => r.DiscoID == discoId);
        }
    }
}