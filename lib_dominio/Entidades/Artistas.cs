using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class Artistas
    {
        [Key] public int ArtistaId { get; set; }
        public string? NombreArtista { get; set; } 
        public string? Nacionalidad { get; set; }
            
        
        [NotMapped] public ICollection<Discos>? Discos { get; set; }
    }
}
