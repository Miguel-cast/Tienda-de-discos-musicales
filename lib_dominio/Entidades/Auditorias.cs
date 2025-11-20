using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class Auditorias
    {
        [Key] public int AuditoriaId { get; set; }
        public DateTime FechaHora { get; set; }
        public int UsuarioId { get; set; }
        public string? Accion { get; set; }
        public string? Tabla { get; set; }

        public Usuarios? Usuario { get; set; }
    }
}

/*
 CREATE TABLE [Auditoria](
    [AuditoriaId] INT IDENTITY(1,1) PRIMARY KEY,
    [FechaHora] DATETIME NOT NULL DEFAULT GETDATE(),
    [UsuarioId] INT NOT NULL,
    [Accion] NVARCHAR(100) NOT NULL, -- INSERT, UPDATE, DELETE, LOGIN, etc.
    [Tabla] NVARCHAR(100), -- Tabla afectada
    CONSTRAINT FK_Auditoria_UsuariosSistema 
        FOREIGN KEY ([UsuarioId]) REFERENCES [UsuariosSistema]([UsuarioId])

    );
GO
 
 */