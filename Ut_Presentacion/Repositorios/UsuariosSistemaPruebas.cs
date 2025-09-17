using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class UsuariosSistemaPruebas
    {
        private readonly IConexion? iConexion;
        private List<UsuariosSistema>? lista;
        private UsuariosSistema? entidad;

        public UsuariosSistemaPruebas()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
        }

        [TestMethod]
        public void Ejecutar()
        {
            Assert.AreEqual(true, Guardar());
            Assert.AreEqual(true, Modificar());
            Assert.AreEqual(true, Listar());
            Assert.AreEqual(true, Borrar());
        }

        public bool Listar()
        {
            this.lista = this.iConexion!.UsuariosSistema!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            this.entidad = new UsuariosSistema
            {
                NombreUsuario = "admin",
                ContrasenaHash = "123456",
                Rol = "Administrador",
                EmpleadoId = 1

            };
            this.iConexion!.UsuariosSistema!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Rol = "Usuario";
            var entry = this.iConexion!.Entry<UsuariosSistema>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.UsuariosSistema!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}