using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IFacturasAplicacion
    {
        void Configurar(string StringConexion);
        List<Facturas> Listar();
        Facturas? Guardar(Facturas? entidad);
        Facturas? Modificar(Facturas? entidad);
        Facturas? Borrar(Facturas? entidad);
        decimal CalcularTotalFacturasCliente(int clienteId);
        List<Facturas> ObtenerFacturasPendientesPago();
        List<Facturas> ObtenerFacturasPorFecha(DateTime fechaInicio, DateTime fechaFin);
        decimal CalcularMontoTotalMes(int año, int mes);
        Facturas? ObtenerFacturaPorPedido(int pedidoId);
    }
}