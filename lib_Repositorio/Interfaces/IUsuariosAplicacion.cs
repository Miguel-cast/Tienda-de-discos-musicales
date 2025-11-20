using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;
using System.Collections.Generic;

namespace lib_repositorios.Interfaces
    {
        public interface IUsuariosAplicacion
        {
            void Configurar(string StringConexion);
            Usuarios? Guardar(Usuarios? entidad);
            Usuarios? Modificar(Usuarios? entidad);
            Usuarios? Borrar(Usuarios? entidad);
            List<Usuarios> Listar();
            List<Usuarios> PorEmail(Usuarios? entidad);
        }

    }

