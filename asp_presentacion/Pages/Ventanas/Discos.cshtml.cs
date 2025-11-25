using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace asp_presentacion.Pages.Ventanas
{
    public class DiscosModel : PageModel
    {
        private IDiscosPresentacion? iPresentacion = null;
        private readonly IArtistasPresentacion? iArtistasPresentacion;
        private readonly IGenerosPresentacion? iGenerosPresentacion;
        private readonly IProveedoresPresentacion? iProveedoresPresentacion;
        public List<SelectListItem> ArtistasSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> GenerosSelectList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ProveedoresSelectList { get; set; } = new List<SelectListItem>();
        public DiscosModel(IDiscosPresentacion iPresentacion, IArtistasPresentacion iArtistasPresentacion,
            IGenerosPresentacion iGenerosPresentacion, IProveedoresPresentacion iProveedoresPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;
                this.iArtistasPresentacion = iArtistasPresentacion;
                this.iGenerosPresentacion = iGenerosPresentacion;
                this.iProveedoresPresentacion = iProveedoresPresentacion;
                Filtro = new Discos();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public IFormFile? FormFile { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Discos? Actual { get; set; }
        [BindProperty] public Discos? Filtro { get; set; }
        [BindProperty] public List<Discos>? Lista { get; set; }
        public virtual void OnGet() 
        {
            CargarListasSecundarias();
            OnPostBtRefrescar();
        }
        private void CargarListasSecundarias()
        {
            try
            {
               
                var taskArtistas = this.iArtistasPresentacion!.Listar();
                taskArtistas.Wait();
                var artistas = taskArtistas.Result;

                var taskGeneros = this.iGenerosPresentacion!.Listar();
                taskGeneros.Wait();
                var generos = taskGeneros.Result;

                var taskProveedores = this.iProveedoresPresentacion!.Listar();
                taskProveedores.Wait();
                var proveedores = taskProveedores.Result;

                if (artistas != null)
                {
                    ArtistasSelectList = artistas.Select(d => new SelectListItem
                    {

                        Value = d.ArtistaId.ToString(),

                        Text = d.NombreArtista
                    }).ToList();
                }  
                
                if(generos != null) {
                    GenerosSelectList = generos.Select(d => new SelectListItem
                    {

                        Value = d.GenerosId.ToString(),

                        Text = d.NombreGenero
                    }).ToList();
                }  
                
                if (proveedores != null) {
                    ProveedoresSelectList = proveedores.Select(d => new SelectListItem
                    {

                        Value = d.ProveedoresId.ToString(),

                        Text = d.NombreEmpresa
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
                Filtro!.Titulo = Filtro!.Titulo ?? "";
                Accion = Enumerables.Ventanas.Listas;

                CargarListasSecundarias();

                var task = this.iPresentacion!.PorTitulo(Filtro!);
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
                Actual = new Discos();
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
                Actual = Lista!.FirstOrDefault(x => x.DiscoId.ToString() == data);
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
                Task<Discos>? task = null;
                if (Actual!.DiscoId == 0)
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
                Actual = Lista!.FirstOrDefault(x => x.DiscoId.ToString() == data);
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