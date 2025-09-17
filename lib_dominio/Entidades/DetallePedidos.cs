
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class DetallePedidos
    {
        [Key] public int DetallesId { get; set; }
        public int cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int PedidoId { get; set; }
        // Propiedad de navegación
        [ForeignKey("PedidoId")]public Pedidos? Pedidos { get; set; }

    }
}

/*
 
CREATE TABLE [DetallePedidos](
	[DetallesId] INT IDENTITY(1,1) PRIMARY KEY,
	[Cantidad] INT NOT NULL,
	[PrecioUnitario] DECIMAL(10,2) NOT NULL,
	[PedidoId] INT NOT NULL,
	CONSTRAINT FK_DetallePedidos_Pedidos 
		FOREIGN KEY ([PedidoId]) REFERENCES [Pedidos]([PedidoID])
);
GO
 */