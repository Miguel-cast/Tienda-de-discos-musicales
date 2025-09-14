
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
		public int GeneroId { get; set; }
		public int ProveedoresId { get; set; }

        // Propiedades de navegación
        [ForeignKey("Artista")] public Artistas? Artista { get; set; }
        [ForeignKey("Genero")] public Generos? Genero { get; set; }
        [ForeignKey("Proveedor")] public Proveedores? Proveedor { get; set; }

        // lista para las relaciones uno a muchos
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