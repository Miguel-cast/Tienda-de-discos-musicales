using lib_Repositorio.Interfaces;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using lib_repositorios.Interfaces.lib_repositorios.Interfaces;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace asp_servicios
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration? Configuration { set; get; }

        public void ConfigureServices(WebApplicationBuilder builder, IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(x => {
                x.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(x => { x.AllowSynchronousIO = true; });
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            //services.AddSwaggerGen();

            // Repositorios
            services.AddScoped<IConexion, Conexion>();
            services.AddScoped<IArtistasAplicacion, ArtistasAplicacion>();
            services.AddScoped<IDiscosAplicacion, DiscosAplicacion>();
            services.AddScoped<ICancionesAplicacion, CancionesAplicacion>();
            services.AddScoped<IProveedoresAplicacion, ProveedoresAplicacion>();
            services.AddScoped<IGenerosAplicacion, GenerosAplicacion>();
            services.AddScoped<IClientesAplicacion, ClientesAplicacion>();
            services.AddScoped<IDetallesPedidosAplicacion, DetallesPedidosAplicacion>();
            services.AddScoped<IEmpleadosAplicacion, EmpleadosAplicacion>();
            services.AddScoped<IEnviosAplicacion, EnviosAplicacion>();
            services.AddScoped<IFacturasAplicacion, FacturasAplicacion>();
            services.AddScoped<IInventarioMovimientosAplicacion, InventarioMovimientosAplicacion>();
            services.AddScoped<IPagosAplicacion, PagosAplicacion>();
            services.AddScoped<IPedidosAplicacion, PedidosAplicacion>();
            services.AddScoped<IReseñasClientesAplicacion, ReseñasClientesAplicacion>();
            services.AddScoped<IUsuariosSistemaAplicacion, UsuariosSistemaAplicacion>();

            // Controladores
            //services.AddScoped<TokenController, TokenController>();
            services.AddCors(o => o.AddDefaultPolicy(b => b.AllowAnyOrigin()));
        }

        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
            app.UseRouting();
            app.UseCors();
        }
    }
}