using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Envios
    {
        [Key] public int EnvioID { get; set; }
        public string? DireccionEntrega { get; set; }
        public string? CiudadEntrega { get; set; }
        public string? PaisEntrega { get; set; }
        public DateTime FechaEnvio { get; set; }
        public int PedidoID { get; set; }

        [ForeignKey("PedidoID")] public Pedidos? Pedido { get; set; }
    }
}

/*
CREATE TABLE [Envios] (
    [EnvioID] INT IDENTITY(1,1) PRIMARY KEY,
    [DireccionEntrega] NVARCHAR(200) NOT NULL,
    [CiudadEntrega] NVARCHAR(100) NOT NULL,
    [PaisEntrega] NVARCHAR(100) NOT NULL,
    [FechaEnvio] DATE NOT NULL,
    [PedidoID] INT NOT NULL,
    CONSTRAINT FK_Envios_Pedidos
        FOREIGN KEY ([PedidoID]) REFERENCES [Pedidos]([PedidoID])
);
GO
*/