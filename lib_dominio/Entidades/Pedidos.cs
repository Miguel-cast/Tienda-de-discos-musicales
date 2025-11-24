using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace lib_dominio.Entidades
{
    public class Pedidos
    {
        [Key] public int PedidoID { get; set; }
        public DateTime FechaPedido { get; set; }
        public string? Estado { get; set; }
        public int ClienteID { get; set; }
        public int EmpleadoID { get; set; }

        public Clientes? Cliente { get; set; }
        public Empleados? Empleado { get; set; }

        [JsonIgnore]
        public ICollection<Facturas>? Facturas { get; set; }
        [JsonIgnore]
        public ICollection<Envios>? Envios { get; set; }
        [JsonIgnore]
        public ICollection<DetallePedidos>? DetallesPedidos { get; set; }
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