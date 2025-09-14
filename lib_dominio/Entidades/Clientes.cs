using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class Clientes
    {
        [Key] public int ClienteId { get; set; }
        public string Nombre { get; set; } 
        public string Apellido { get; set; } 
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Ciudad { get; set; }
        public string? Pais { get; set; }

        
        [NotMapped] public ICollection<Pedidos>? Pedidos { get; set; }
        [NotMapped] public ICollection<ReseñasClientes>? ReseñasClientes { get; set; }
    }
}
