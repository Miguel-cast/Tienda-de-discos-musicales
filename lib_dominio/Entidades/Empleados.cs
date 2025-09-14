using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class Empleados
    {
        [Key] public int EmpleadoId { get; set; }
        public string Nombre { get; set; } 
        public string Apellido { get; set; } 
        public string? Cargo { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }

        [NotMapped] public ICollection<Pedidos>? Pedidos { get; set; }
        [NotMapped] public ICollection<UsuariosSistema>? UsuariosSistema { get; set; }
        [NotMapped] public ICollection<InventarioMovimientos>? InventarioMovimientos { get; set; }

    }
}
