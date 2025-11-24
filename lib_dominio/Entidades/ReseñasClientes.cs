using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class ReseñasClientes
    {
        [Key] public int ReseñaID { get; set; }
        public string? Comentario { get; set; }
        public int Calificacion { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteID { get; set; }  
        public int DiscoID { get; set; }

        public Clientes? Cliente { get; set; }
        public Discos? Disco { get; set; }
    }
}

/*
CREATE TABLE [ReseñasClientes] (
    [ReseñaID] INT IDENTITY(1,1) PRIMARY KEY,
    [Comentario] NVARCHAR(500) NOT NULL,
    [Calificacion] INT CHECK ([Calificacion] BETWEEN 1 AND 5),
    [Fecha] DATE NOT NULL,
    [ClienteID] INT NOT NULL,
    [DiscoID] INT NOT NULL,
    CONSTRAINT FK_ReseñasClientes_Clientes
        FOREIGN KEY ([ClienteID]) REFERENCES [Clientes]([ClienteID]),
    CONSTRAINT FK_ReseñasClientes_Discos
        FOREIGN KEY ([DiscoID]) REFERENCES [Discos]([DiscoID])
);
GO
*/