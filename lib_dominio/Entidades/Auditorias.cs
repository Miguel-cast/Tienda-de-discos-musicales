
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lib_dominio.Entidades
{
    public class Auditorias
    {
        [Key] public int AuditoriaId { get; set; }

        public DateTime Fecha { get; set; } 

        public string? Usuario { get; set; }

        
        public string? Accion { get; set; }

        
        public string? Tabla { get; set; }

        public string? Descripcion { get; set; }

      
    }
}