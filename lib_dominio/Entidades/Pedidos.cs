using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Pedidos
    {
        [Key] public int PedidoID { get; set; }
        public DateTime FechaPedido { get; set; }
        public string? Estado { get; set; }
        public int ClienteID { get; set; }
        public int EmpleadoID { get; set; }

        [ForeignKey("ClienteID")] public Clientes? Cliente { get; set; }
        [ForeignKey("EmpleadoID")] public Empleados? Empleado { get; set; }

        // Listas de entidades relacionadas a la tabla
        public ICollection<Facturas>? Facturas { get; set; }
        public ICollection<Envios>? Envios { get; set; }
        public ICollection<DetallesPedidos>? DetallesPedidos { get; set; }
    }
}

/*
CREATE TABLE [Pedidos] (
    [PedidoID] INT IDENTITY(1,1) PRIMARY KEY,
    [FechaPedido] DATE NOT NULL,
    [Estado] NVARCHAR(50) NOT NULL,
    [ClienteID] INT NOT NULL,
    [EmpleadoID] INT NOT NULL,
    CONSTRAINT FK_Pedidos_Clientes
        FOREIGN KEY ([ClienteID]) REFERENCES [Clientes]([ClienteID]),
    CONSTRAINT FK_Pedidos_Empleados
        FOREIGN KEY ([EmpleadoID]) REFERENCES [Empleados]([EmpleadoID])
);
GO
*/