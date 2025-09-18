using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_repositorios.Interfaces
{
    using lib_dominio.Entidades;
    using System.Collections.Generic;

    namespace lib_repositorios.Interfaces
    {
        public interface IUsuariosSistemaAplicacion
        {
            void Configurar(string StringConexion);
            UsuariosSistema? Guardar(UsuariosSistema? entidad);
            UsuariosSistema? Modificar(UsuariosSistema? entidad);
            UsuariosSistema? Borrar(UsuariosSistema? entidad);
            List<UsuariosSistema> Listar();
        }

    }
}
