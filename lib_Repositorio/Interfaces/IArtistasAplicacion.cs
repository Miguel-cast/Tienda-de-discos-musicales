using lib_dominio.Entidades;
using lib_repositorios.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lib_repositorios.Interfaces
{
    public interface IArtistasAplicacion
    {
        void Configurar(string StringConexion);
        List<Artistas> Listar();
        List<Artistas> PorNombreArtista(Artistas? entidad);
        Artistas? Guardar(Artistas? entidad);
        Artistas? Modificar(Artistas? entidad);
        Artistas? Borrar(Artistas? entidad);
    }
}
