
using lib_dominio.Entidades;
using static lib_dominio.Nucleo.Enumerables;

namespace ut_presentacion.Nucleo
{
    public class EntidadesNucleo
    {
        public static Discos? Discos()
        {
            var entidad = new Discos();
            entidad.Titulo = "Un verano sin ti";
            entidad.AñoLanzamiento = 2023;
            entidad.Precio = 59900.00m;
            entidad.ArtistaId = 4;
            entidad.GenerosId = 1;
            entidad.ProveedoresId = 2;

            return entidad;
        }

        public static Canciones? Canciones()
        {
            var entidad = new Canciones();
            entidad.Titulo = "Waka Waka";
            entidad.Duracion = new TimeOnly(0, 3, 26);
            entidad.DiscoID = 2;

            return entidad;
        }

        public static Artistas? Artistas()
        {
            var entidad = new Artistas();
            entidad.NombreArtista = "Bad Boony";
            entidad.Nacionalidad = "Puerto Rico";

            return entidad;
        }


        public static Generos? Generos()
        {
            var entidad = new Generos();
            entidad.NombreGenero = "Vallenato";
            entidad.Descripcion = "Musica tipica de la costa caribe colombiana";

            return entidad;
        }

        public static Clientes? Clientes()
        {
            var entidad = new Clientes();
            entidad.Nombre = "Luciano";
            entidad.Apellido = "Padilla";
            entidad.Email = "lucipa12@gmail.com";
            entidad.Telefono = "3104567890";
            entidad.Direccion = "Cra 7 # 45-67";
            entidad.Ciudad = "Madrid";
            entidad.Pais = "España";

            return entidad;
        }

        public static InventarioMovimientos? InventarioMovimientos()
        {
            var entidad = new InventarioMovimientos();
            entidad.FechaMovimiento = DateTime.Now;
            entidad.TipoMovimiento = "Entrada";
            entidad.Cantidad = 50;
            entidad.DiscoId = 3;
            entidad.EmpleadoId = 1;

            return entidad;
        }

        public static Proveedores? Proveedores()
        {
            var entidad = new Proveedores();
            entidad.NombreEmpresa = "Music World S.A.S"; 
            entidad.Contacto = "Ana Maria Lopez";
            entidad.Telefono = "6012345678";
            entidad.Direccion = "Calle 123 #45-67, Medellin, Colombia";

            return entidad;
        }

    }
}