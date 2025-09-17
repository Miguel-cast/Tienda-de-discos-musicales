

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Canciones
    {
        [Key] public int CancionId { get; set; }
        public string? Titulo { get; set; }
        public TimeOnly Duracion { get; set; }
        public int DiscoID { get; set; }


        // Propiedad de navegación
       public Discos? Disco { get; set; }

    }
}

/*
 CREATE TABLE [Canciones](
	[CancionId] INT IDENTITY(1,1) PRIMARY KEY,
	[Titulo] NVARCHAR(100) NOT NULL,
	[Duracion] TIME NOT NULL,
	[DiscoID] INT NOT NULL,
	CONSTRAINT FK_Canciones_Discos 
		FOREIGN KEY ([DiscoId]) REFERENCES [Discos]([DiscoID])
);
GO
 */