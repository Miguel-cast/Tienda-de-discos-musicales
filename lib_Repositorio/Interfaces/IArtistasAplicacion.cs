using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace lib_repositorios.Interfaces
{
    public interface IArtistasAplicacion
    {
        void Configurar(string StringConexion);
        List<Artistas> Listar();
        Artistas? Guardar(Artistas? entidad);
        Artistas? Modificar(Artistas? entidad);
        Artistas? Borrar(Artistas? entidad);
    }
}
