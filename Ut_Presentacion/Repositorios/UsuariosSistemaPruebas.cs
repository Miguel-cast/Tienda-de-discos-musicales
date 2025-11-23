using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Repositorios
{
    [TestClass]
    public class UsuariosPruebas
    {
        private readonly IConexion? iConexion;
        private List<Usuarios>? lista;
        private Usuarios? entidad;

        public UsuariosPruebas()
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
            this.lista = this.iConexion!.Usuarios!.ToList();
            return lista.Count > 0;
        }

        public bool Guardar()
        {
            this.entidad = new Usuarios
            {
                N = "admin",
                ContrasenaHash = "123456",
                Rol = "Administrador",
                EmpleadoId = 1

            };
            this.iConexion!.Usuarios!.Add(this.entidad);
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Modificar()
        {
            this.entidad!.Rol = "Usuario";
            var entry = this.iConexion!.Entry<Usuarios>(this.entidad);
            entry.State = EntityState.Modified;
            this.iConexion!.SaveChanges();
            return true;
        }

        public bool Borrar()
        {
            this.iConexion!.Usuarios!.Remove(this.entidad!);
            this.iConexion!.SaveChanges();
            return true;
        }
    }
}