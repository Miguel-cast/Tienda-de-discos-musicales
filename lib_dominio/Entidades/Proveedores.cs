
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Proveedores
    {
        [Key] public int ProveedoresId { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? Contacto { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }

        // lista para las relaciones uno a muchos
        [NotMapped] public ICollection<Discos>? Discos { get; set; }

    }
}

/*
 CREATE TABLE [Proveedores](
	[ProveedoresId] INT IDENTITY (1,1) PRIMARY KEY,
	[NombreEmpresa] NVARCHAR (100) NOT NULL,
	[Contacto] NVARCHAR (50) NOT NULL,
	[Telefono] NVARCHAR (20) NOT NULL,
	[Direccion] NVARCHAR (200),
);

GO
 */