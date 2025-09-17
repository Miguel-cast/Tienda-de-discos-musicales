using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IPagosAplicacion
    {
        void Configurar(string StringConexion);
        List<Pagos> Listar();
        Pagos? Guardar(Pagos? entidad);
        Pagos? Modificar(Pagos? entidad);
        Pagos? Borrar(Pagos? entidad);
        List<Pagos> ObtenerPagosPorFactura(int facturaId);
        decimal CalcularTotalPagosPorMetodo(string metodoPago);
        List<Pagos> ObtenerPagosPorFecha(DateTime fechaInicio, DateTime fechaFin);
        decimal CalcularSaldoPendienteFactura(int facturaId);
        List<Pagos> ObtenerPagosRecientes(int dias = 7);
    }
}