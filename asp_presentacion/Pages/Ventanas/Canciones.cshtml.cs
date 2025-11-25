using lib_dominio.Entidades;
using lib_dominio.Nucleo;
using lib_presentaciones.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering; // ¡Necesario para SelectListItem!

namespace asp_presentacion.Pages.Ventanas
{
    public class CancionesModel : PageModel
    {
        
        private ICancionesPresentacion? iPresentacion = null;

        // Interfaz NUEVA: Necesaria para obtener la lista de Discos (FK)
        private readonly IDiscosPresentacion? iDiscosPresentacion;

        // NUEVA PROPIEDAD: Lista para llenar el Dropdown (Select)
        public List<SelectListItem> DiscosSelectList { get; set; } = new List<SelectListItem>();


        // Constructor modificado para inyectar AMBAS interfaces
        public CancionesModel(ICancionesPresentacion iPresentacion, IDiscosPresentacion iDiscosPresentacion)
        {
            try
            {
                this.iPresentacion = iPresentacion;
                this.iDiscosPresentacion = iDiscosPresentacion; // Asignación de la nueva interfaz
                Filtro = new Canciones();
            }
            catch (Exception ex)
            {
                LogConversor.Log(ex, ViewData!);
            }
        }

        public IFormFile? FormFile { get; set; }
        [BindProperty] public Enumerables.Ventanas Accion { get; set; }
        [BindProperty] public Canciones? Actual { get; set; }
        [BindProperty] public Canciones? Filtro { get; set; }
        [BindProperty] public List<Canciones>? Lista { get; set; }

        // Método principal de carga de página (OnGet)
        public virtual void OnGet()
        {
            CargarListasSecundarias(); // Llamamos a la nueva función
            OnPostBtRefrescar();
        }

        // --- Nuevo método para cargar datos de claves foráneas ---
        private void CargarListasSecundarias()
        {
            try
            {
                // 1. Cargar la lista de Discos desde el servicio
                var taskDiscos = this.iDiscosPresentacion!.Listar();
                taskDiscos.Wait();
                var discos = taskDiscos.Result;

                // 2. Convertir la lista de Discos a SelectListItem
                if (discos != null)
                {
                    DiscosSelectList = discos.Select(d => new SelectListItem
                    {
                        // Se asume que el ID del Disco es DiscoID
                        Value = d.DiscoId.ToString(),
                        // Se asume que el texto a mostrar es Titulo
                        Text = d.Titulo
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                // Manejar errores si la DB de Discos falla
                LogConversor.Log(ex, ViewData!);
            }
        }
        // -----------------------------------------------------------


        public void OnPostBtRefrescar()
        {
            try
            {
                // ... (lógica de sesión comentada)
                Filtro!.Titulo = Filtro!.Titulo ?? "";
                Accion = Enumerables.Ventanas.Listas;

                // Aseguramos que las listas secundarias estén cargadas antes de refrescar
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
                Actual = new Canciones();
                CargarListasSecundarias(); // Cargar listas al crear uno nuevo
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
                Actual = Lista!.FirstOrDefault(x => x.CancionId.ToString() == data);
                CargarListasSecundarias(); // Cargar listas al modificar
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
                Task<Canciones>? task = null;
                if (Actual!.CancionId == 0)
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
                Actual = Lista!.FirstOrDefault(x => x.CancionId.ToString() == data);
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