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
            entidad.ArtistaId = 1;
            entidad.GenerosId = 1;
            entidad.ProveedoresId = 1;

            return entidad;
        }

        public static Canciones? Canciones()
        {
            var entidad = new Canciones();
            entidad.Titulo = "Waka Waka";
            entidad.Duracion = new TimeOnly(0, 3, 26);
            entidad.DiscoID = 1;

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
            entidad.FechaMovimiento = DateTime.Now.Date; // ← Corregido
            entidad.TipoMovimiento = "Entrada";
            entidad.Cantidad = 50;
            entidad.DiscoId = 1;
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

        public static DetallePedidos? DetallePedidos()
        {
            var entidad = new DetallePedidos();
            entidad.cantidad = 70;
            entidad.PrecioUnitario = 45900.00m;
            entidad.PedidoId = 1;

            return entidad;
        }

        public static Pedidos? Pedidos()
        {
            var entidad = new Pedidos();
            entidad.FechaPedido = DateTime.Now.Date; // ← Corregido
            entidad.Estado = "En Proceso";
            entidad.ClienteID = 1;
            entidad.EmpleadoID = 1;

            return entidad;
        }

        public static Pagos? Pagos()
        {
            var entidad = new Pagos();
            entidad.FechaPago = DateTime.Now.Date; // ← Corregido
            entidad.Monto = 125500.00m;
            entidad.MetodoPago = "Tarjeta de Crédito";
            entidad.FacturaID = 1;

            return entidad;
        }

        public static Envios? Envios()
        {
            var entidad = new Envios();
            entidad.DireccionEntrega = "Calle 45 #12-34";
            entidad.CiudadEntrega = "Bogotá";
            entidad.PaisEntrega = "Colombia";
            entidad.FechaEnvio = DateTime.Now.Date.AddDays(1); // ← Corregido
            entidad.PedidoID = 1;

            return entidad;
        }

        public static ReseñasClientes? ReseñasClientes()
        {
            var entidad = new ReseñasClientes();
            entidad.Comentario = "Excelente álbum, me encantaron todas las canciones.";
            entidad.Calificacion = 5;
            entidad.Fecha = DateTime.Now.Date; // ← Corregido
            entidad.ClienteID = 1;
            entidad.DiscoID = 1;

            return entidad;
        }

        public static UsuariosSistema? UsuariosSistema()
        {
            var entidad = new UsuariosSistema();
            entidad.NombreUsuario = "admin";
            entidad.ContrasenaHash = "hashed_password";
            entidad.Rol = "Administrador";
            entidad.EmpleadoId = 1;

            return entidad;
        }

        public static Facturas? Facturas()
        {
            var entidad = new Facturas();
            entidad.FechaFactura = DateTime.Now.Date; // ← Corregido
            entidad.Total = 150000.00m;
            entidad.PedidoID = 1;

            return entidad;
        }

        public static Empleados? Empleados()
        {
            var entidad = new Empleados();
            entidad.Nombre = "Felipe";
            entidad.Apellido = "Suluaga";
            entidad.Cargo = "Gerente";
            entidad.Email = "felisulu@gmail.com";
            entidad.Telefono = "3216549870";

            return entidad;
        }
    }
}