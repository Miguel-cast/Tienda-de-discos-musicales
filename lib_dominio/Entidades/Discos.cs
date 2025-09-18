
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Discos
    {
		[Key] public int DiscoId { get; set; }
		public string? Titulo { get; set; }
		public int AñoLanzamiento { get; set; }
		public decimal Precio { get; set; }
		public int ArtistaId { get; set; }
		public int GenerosId { get; set; }
		public int ProveedoresId { get; set; }

        public Artistas? Artista { get; set; }
        public Generos? Genero { get; set; }
        public Proveedores? Proveedor { get; set; }

        [NotMapped] public ICollection<ReseñasClientes>? ReseñasClientes { get; set; }
        [NotMapped] public ICollection<InventarioMovimientos>? InventarioMovimientos { get; set; }
        [NotMapped] public ICollection<Canciones>? Canciones{ get; set; }
        
    }
}

/*
 CREATE TABLE [Discos] (
	[DiscoId] INT IDENTITY (1,1) PRIMARY KEY,
	[Titulo] NVARCHAR(50) NOT NULL,
	[AñoLanzamiento] INT,
	[Precio] DECIMAL(10,2) NOT NULL,
);
GO

-- Tabla Discos

ALTER TABLE [Discos] 
	ADD [ArtistaId] INT CONSTRAINT FK_Discos_Artistas 
			FOREIGN KEY REFERENCES [Artistas]([ArtistaId]),
		[Id] INT CONSTRAINT FK_Discos_ 
			FOREIGN KEY REFERENCES []([Id]),
		[ProveedoresId] INT CONSTRAINT FK_Discos_Proveedores 
			FOREIGN KEY REFERENCES [Proveedores]([ProveedoresId]);
GO
 */