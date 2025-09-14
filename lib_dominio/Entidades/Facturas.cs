using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Facturas
    {
        [Key] public int FacturaID { get; set; }
        public DateTime FechaFactura { get; set; }
        public decimal Total { get; set; }
        public int PedidoID { get; set; }

        [ForeignKey("PedidoID")] public Pedidos? Pedido { get; set; }

        // Lista de pagos asociados a esta tabla factura
        public ICollection<Pagos>? Pagos { get; set; }
    }
}

/*
CREATE TABLE [Facturas] (
    [FacturaID] INT IDENTITY(1,1) PRIMARY KEY,
    [FechaFactura] DATE NOT NULL,
    [Total] DECIMAL(10,2) NOT NULL,
    [PedidoID] INT NOT NULL,
    CONSTRAINT FK_Facturas_Pedidos
        FOREIGN KEY ([PedidoID]) REFERENCES [Pedidos]([PedidoID])
);
GO
*/