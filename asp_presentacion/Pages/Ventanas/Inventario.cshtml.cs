using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace asp_presentacion.Pages.Ventanas
{
    public class InventarioMovimientosModel : PageModel
    {
        private IInventarioMovimientosPresentacion? iPresentacion = null;
        private readonly IDiscosPresentacion? iDiscosPresentacion;
        private readonly IEmpleadosPresentacion? iEmpleadosPresentacion;

        public List<SelectListItem> DiscosSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> EmpleadosSelectList { get; set; } = new List<SelectListItem>();

        public InventarioMovimientosModel(IInventarioMovimientosPresentacion iPresentacion, IDiscosPresentacion iDiscosPresentacion,
            IEmpleadosPresentacion iEmpleadosPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;
                this.iDiscosPresentacion = iDiscosPresentacion;
                this.iEmpleadosPresentacion = iEmpleadosPresentacion;

                Filtro = new InventarioMovimientos();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public IFormFile? FormFile { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public InventarioMovimientos? Actual { get; set; }
        [BindProperty] public InventarioMovimientos? Filtro { get; set; }
        [BindProperty] public List<InventarioMovimientos>? Lista { get; set; }
        public virtual void OnGet() 
        {
            CargarListasSecundarias();
            OnPostBtRefrescar();
        }
        private void CargarListasSecundarias()
        {
            try
            {
               
                var taskDiscos = this.iDiscosPresentacion!.Listar();
                taskDiscos.Wait();
                var discos = taskDiscos.Result;

                var taskEmpleados = this.iEmpleadosPresentacion!.Listar();
                taskEmpleados.Wait();
                var empleados = taskEmpleados.Result;



                if (discos != null)
                {
                    DiscosSelectList = discos.Select(d => new SelectListItem
                    {

                        Value = d.ArtistaId.ToString(),

                        Text = d.Titulo
                    }).ToList();
                }  
                
                if(empleados != null) {
                    EmpleadosSelectList = empleados.Select(d => new SelectListItem
                    {

                        Value = d.EmpleadoId.ToString(),

                        Text = d.Nombre
                    }).ToList();
                }  
                

            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
        public void OnPostBtRefrescar()
        {
            try
            {
                try
                {
                    // var variable_session = HttpContext.Session.GetString("Usuario");
                    // if (String.IsNullOrEmpty(variable_session))
                    //{
                    //       HttpContext.Response.Redirect("/");
                    //        return;
                    //     }
                    Accion = Enumerables.Ventanas.Listas;
                    var task = this.iPresentacion!.Listar();
                    task.Wait();
                    Lista = task.Result;
                    Actual = null;
                }
                catch (Exception ex)
                {
                    LogConversor.Log(ex, ViewData!);
                }
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public virtual void OnPostBtNuevo()
        {
            try
            {
                Accion = Enumerables.Ventanas.Editar;
                Actual = new InventarioMovimientos();
                CargarListasSecundarias();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public virtual void OnPostBtModificar(string data)
        {
            try
            {
                OnPostBtRefrescar();
                Accion = Enumerables.Ventanas.Editar;
                Actual = Lista!.FirstOrDefault(x => x.MovimientoId.ToString() == data);
                CargarListasSecundarias();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public virtual void OnPostBtGuardar()
        {
            try
            {
                Accion = Enumerables.Ventanas.Editar;
                Task<InventarioMovimientos>? task = null;
                if (Actual!.MovimientoId == 0)
                    task = this.iPresentacion!.Guardar(Actual!)!;
                else
                    task = this.iPresentacion!.Modificar(Actual!)!;
                task.Wait();
                Actual = task.Result;
                Accion = Enumerables.Ventanas.Listas;
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public virtual void OnPostBtBorrarVal(string data)
        {
            try
            {
                OnPostBtRefrescar();
                Accion = Enumerables.Ventanas.Borrar;
                Actual = Lista!.FirstOrDefault(x => x.MovimientoId.ToString() == data);
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public virtual void OnPostBtBorrar()
        {
            try
            {
                var task = this.iPresentacion!.Borrar(Actual!);
                Actual = task.Result;
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtCancelar()
        {
            try
            {
                Accion = Enumerables.Ventanas.Listas;
                OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public void OnPostBtCerrar()
        {
            try
            {
                if (Accion == Enumerables.Ventanas.Listas)
                    OnPostBtRefrescar();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }
    }
}