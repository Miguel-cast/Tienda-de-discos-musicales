using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_dominio.Entidades
{
    public class InventarioMovimientos
    {
        [Key] public int MovimientoId { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public string? TipoMovimiento { get; set; } 
        public int Cantidad { get; set; }
        public int DiscoId { get; set; }
        public int EmpleadoId { get; set; }
        [ForeignKey("DiscoId")] public Discos? Disco { get; set; }
        [ForeignKey("EmpleadoId")] public Empleados? Empleado { get; set; }
    }
}
