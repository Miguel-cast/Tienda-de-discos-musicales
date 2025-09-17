using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Pagos
    {
        [Key] public int PagoID { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string? MetodoPago { get; set; }
        public int FacturaID { get; set; }

        public Facturas? Factura { get; set; }
    }
}

/*
CREATE TABLE [Pagos] (
    [PagoID] INT IDENTITY(1,1) PRIMARY KEY,
    [FechaPago] DATE NOT NULL,
    [Monto] DECIMAL(10,2) NOT NULL,
    [MetodoPago] NVARCHAR(50) NOT NULL,
    [FacturaID] INT NOT NULL,
    CONSTRAINT FK_Pagos_Facturas
        FOREIGN KEY ([FacturaID]) REFERENCES [Facturas]([FacturaID])
);
GO
*/