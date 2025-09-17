﻿using lib_dominio.Entidades;

namespace lib_repositorios.Interfaces
{
    public interface IEnviosAplicacion
    {
        void Configurar(string StringConexion);
        List<Envios> Listar();
        Envios? Guardar(Envios? entidad);
        Envios? Modificar(Envios? entidad);
        Envios? Borrar(Envios? entidad);
        List<Envios> ObtenerEnviosPorCiudad(string ciudad);
        List<Envios> ObtenerEnviosPorFecha(DateTime fechaInicio, DateTime fechaFin);
        List<Envios> ObtenerEnviosPendientes();
        List<Envios> ObtenerEnviosRealizados();
        int ContarEnviosPorPais(string pais);
        Envios? ObtenerEnvioPorPedido(int pedidoId);
    }
}