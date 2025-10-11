using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class UsuariosSistema
    {
        [Key]
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string ContrasenaHash { get; set; }
        public int EmpleadoId { get; set; }
        public int RolId { get; set; }
        public Empleados? Empleado { get; set; }
        public Roles? Rol { get; set; }
    }

}
