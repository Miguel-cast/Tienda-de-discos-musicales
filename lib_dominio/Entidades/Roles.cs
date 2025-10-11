using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class Roles
    {
        [Key] public int RolId { get; set; }
        public string? NombreRol{ get; set; }
        public string? Descripcion { get; set; }
    }
}

/*
CREATE TABLE [Roles](
    [RolId] INT IDENTITY(1,1) PRIMARY KEY,
    [NombreRol] NVARCHAR(50) NOT NULL UNIQUE,
    [Descripcion] NVARCHAR(200)
    );
GO
 
 */