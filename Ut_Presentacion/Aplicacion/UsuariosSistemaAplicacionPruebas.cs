using lib_dominio.Entidades;
using lib_repositorios.Implementaciones;
using lib_repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ut_presentacion.Nucleo;

namespace ut_presentacion.Aplicacion
{
    [TestClass]
    public class UsuariosSistemaAplicacionPruebas
    {
        private IConexion? iConexion;
        private UsuariosSistemaAplicacion? aplicacion;
        private UsuariosSistema? entidad;

        [TestInitialize]
        public void Inicializar()
        {
            iConexion = new Conexion();
            iConexion.StringConexion = Configuracion.ObtenerValor("StringConexion");
            aplicacion = new UsuariosSistemaAplicacion(iConexion);
        }

        [TestMethod]
        public void PruebaGuardarUsuarioValido()
        {
            entidad = EntidadesNucleo.UsuariosSistema();
            entidad.NombreUsuario = "usuario_prueba";
            entidad.ContrasenaHash = "password123";
            entidad.Rol = "Administrador";
            var resultado = aplicacion!.Guardar(entidad);
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.UsuarioId > 0);
        }


        [TestCleanup]
        public void LimpiarDatos()
        {
            if (entidad != null && entidad.UsuarioId > 0)
            {
                var usuarioExistente = iConexion!.UsuariosSistema!.Find(entidad.UsuarioId);
                if (usuarioExistente != null)
                {
                    iConexion.UsuariosSistema.Remove(usuarioExistente);
                    iConexion.SaveChanges();
                }
            }
        }
    }
}
