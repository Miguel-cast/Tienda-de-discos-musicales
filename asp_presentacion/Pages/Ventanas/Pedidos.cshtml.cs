using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace asp_presentacion.Pages.Ventanas
{
    public class PedidosModel : PageModel
    {
        private IPedidosPresentacion? iPresentacion = null;
        private readonly IClientesPresentacion? iClientesPresentacion;
        private readonly IEmpleadosPresentacion? iEmpleadosPresentacion;

        public List<SelectListItem> ClientesSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> EmpleadosSelectList { get; set; } = new List<SelectListItem>();
        public PedidosModel(IPedidosPresentacion iPresentacion, IClientesPresentacion iClientesPresentacion,
            IEmpleadosPresentacion iEmpleadosPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;
                this.iClientesPresentacion = iClientesPresentacion;
                this.iEmpleadosPresentacion = iEmpleadosPresentacion;
                Filtro = new Pedidos();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public IFormFile? FormFile { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Pedidos? Actual { get; set; }
        [BindProperty] public Pedidos? Filtro { get; set; }
        [BindProperty] public List<Pedidos>? Lista { get; set; }
        public virtual void OnGet() 
        {
            CargarListasSecundarias();
            OnPostBtRefrescar();
        }
        private void CargarListasSecundarias()
        {
            try
            {
               
                var taskClientes = this.iClientesPresentacion!.Listar();
                taskClientes.Wait();
                var clientes = taskClientes.Result;

                var taskEmpleados = this.iEmpleadosPresentacion!.Listar();
                taskEmpleados.Wait();
                var empleados = taskEmpleados.Result;



                if (clientes != null)
                {
                    ClientesSelectList = clientes.Select(d => new SelectListItem
                    {

                        Value = d.ClienteId.ToString(),

                        Text = d.Nombre
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
                //var variable_session = HttpContext.Session.GetString("Usuario");
                //if (String.IsNullOrEmpty(variable_session))
                //{
                //    HttpContext.Response.Redirect("/");
                //    return;
                //}
                Filtro!.Estado = Filtro!.Estado ?? "";
                Accion = Enumerables.Ventanas.Listas;

                CargarListasSecundarias();

                var task = this.iPresentacion!.PorEstado(Filtro!);
                task.Wait();
                Lista = task.Result;
                Actual = null;
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
                Actual = new Pedidos();
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
                Actual = Lista!.FirstOrDefault(x => x.PedidoID.ToString() == data);
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
                Task<Pedidos>? task = null;
                if (Actual!.PedidoID == 0)
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
                Actual = Lista!.FirstOrDefault(x => x.PedidoID.ToString() == data);
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