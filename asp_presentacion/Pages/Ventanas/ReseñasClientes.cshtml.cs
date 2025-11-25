using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace asp_presentacion.Pages.Ventanas
{
    public class ReseñasClientesModel : PageModel
    {
        private IReseñasClientesPresentacion? iPresentacion = null;
        private readonly IClientesPresentacion? iClientesPresentacion;
        private readonly IDiscosPresentacion? iDiscosPresentacion;
        private readonly IProveedoresPresentacion? iProveedoresPresentacion;
        public List<SelectListItem> ClientesSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> DiscosSelectList { get; set; } = new List<SelectListItem>();
        public ReseñasClientesModel(IReseñasClientesPresentacion iPresentacion, IClientesPresentacion iClientesPresentacion,
            IDiscosPresentacion iDiscosPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;
                this.iClientesPresentacion = iClientesPresentacion;
                this.iDiscosPresentacion = iDiscosPresentacion;
                Filtro = new ReseñasClientes();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public IFormFile? FormFile { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public ReseñasClientes? Actual { get; set; }
        [BindProperty] public ReseñasClientes? Filtro { get; set; }
        [BindProperty] public List<ReseñasClientes>? Lista { get; set; }
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

                var taskDiscos = this.iDiscosPresentacion!.Listar();
                taskDiscos.Wait();
                var discos = taskDiscos.Result;


                if (clientes != null)
                {
                    ClientesSelectList = clientes.Select(d => new SelectListItem
                    {

                        Value = d.ClienteId.ToString(),

                        Text = d.Nombre
                    }).ToList();
                }

                if (discos != null)
                {
                    DiscosSelectList = discos.Select(d => new SelectListItem
                    {

                        Value = d.DiscoId.ToString(),

                        Text = d.Titulo
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

        public virtual void OnPostBtNuevo()
        {
            try
            {
                Accion = Enumerables.Ventanas.Editar;
                Actual = new ReseñasClientes();
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
                Actual = Lista!.FirstOrDefault(x => x.ReseñaID.ToString() == data);
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
                Task<ReseñasClientes>? task = null;
                if (Actual!.ReseñaID == 0)
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
                Actual = Lista!.FirstOrDefault(x => x.ReseñaID.ToString() == data);
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