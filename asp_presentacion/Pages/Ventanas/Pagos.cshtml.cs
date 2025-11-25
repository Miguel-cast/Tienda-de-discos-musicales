using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace asp_presentacion.Pages.Ventanas
{
    public class PagosModel : PageModel
    {
        private IPagosPresentacion? iPresentacion = null;
        private readonly IFacturasPresentacion? iFacturasPresentacion;

        public List<SelectListItem> FacturasSelectList { get; set; } = new List<SelectListItem>();
        public PagosModel(IPagosPresentacion iPresentacion, IFacturasPresentacion iFacturasPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;
                this.iFacturasPresentacion = iFacturasPresentacion;
                Filtro = new Pagos();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public IFormFile? FormFile { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Pagos? Actual { get; set; }
        [BindProperty] public Pagos? Filtro { get; set; }
        [BindProperty] public List<Pagos>? Lista { get; set; }
        public virtual void OnGet()
        {
            CargarListasSecundarias(); // Llamamos a la nueva función
            OnPostBtRefrescar();
        }

        private void CargarListasSecundarias()
        {
            try
            {

                var taskFacturas = this.iFacturasPresentacion!.Listar();
                taskFacturas.Wait();
                var facturas = taskFacturas.Result;


                if (facturas != null)
                {
                    FacturasSelectList = facturas.Select(d => new SelectListItem
                    {

                        Value = d.FacturaID.ToString(),

                        Text = d.FechaFactura.ToString("g")
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
                //    //var variable_session = HttpContext.Session.GetString("Usuario");
                //    //if (String.IsNullOrEmpty(variable_session))
                //    //{
                //    //    HttpContext.Response.Redirect("/");
                //    //    return;
                //    //}
                //    Filtro!.Titulo = Filtro!.Titulo ?? "";
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
                Actual = new Pagos();
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
                Actual = Lista!.FirstOrDefault(x => x.FacturaID.ToString() == data);
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
                Task<Pagos>? task = null;
                if (Actual!.FacturaID == 0)
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
                Actual = Lista!.FirstOrDefault(x => x.FacturaID.ToString() == data);
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