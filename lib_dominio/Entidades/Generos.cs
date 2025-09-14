
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Generos
    {
        public int GenerosId { get; set; }
        public string? NombreGenero { get; set; } 
        public string? Descripcion { get; set; }

        // lista para las relaciones uno a muchos
        [NotMapped] public ICollection<Discos>? Discos { get; set; }
    }
}


/*
 CREATE TABLE [](
	[Id] INT IDENTITY (1,1) PRIMARY KEY,
	[NombreGenero] NVARCHAR (50) NOT NULL,
	[Descripcion]  NVARCHAR (500) 
);

GO
*/