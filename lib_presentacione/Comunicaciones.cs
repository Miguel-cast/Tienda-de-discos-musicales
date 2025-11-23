using lib_dominio.Nucleo;
using lib_dominio.Entidades;

namespace lib_presentaciones
{
    public class Comunicaciones
    {
        private string? URL = string.Empty,
            llave = null;

        public Comunicaciones(string url = "http://localhost:5203/")
        {
            URL = url;
        }

        public Dictionary<string, object> ConstruirUrl(Dictionary<string, object> data, string Metodo)
        {
            data["Url"] = URL + Metodo;
            data["UrlLlave"] = URL + "Token/Llave";
            return data;
        }

        public async Task<Dictionary<string, object>> Ejecutar(Dictionary<string, object> datos)
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                respuesta = await Llave(datos);
                if (respuesta == null || respuesta.ContainsKey("Error"))
                    return respuesta!;
                respuesta.Clear();

                var url = datos["Url"].ToString();
                datos.Remove("Url");
                datos.Remove("UrlLlave");
                datos["Llave"] = llave!;
                var stringData = JsonConversor.ConvertirAString(datos);

                using var httpClient = new HttpClient(); // Usamos 'using' para asegurar Dispose
                httpClient.Timeout = new TimeSpan(0, 4, 0);
                var message = await httpClient.PostAsync(url, new StringContent(stringData));

                // --- CAMBIO CLAVE: Leer la respuesta inmediatamente ---
                var resp = await message.Content.ReadAsStringAsync();

                if (!message.IsSuccessStatusCode)
                {
                    // Si el servidor devolvió un error (4xx, 5xx), agregamos la respuesta cruda al error.
                    respuesta.Add("Error", $"lbErrorComunicacion. Status: {message.StatusCode}. Respuesta Cruda: {resp.Substring(0, Math.Min(resp.Length, 100))}");
                    return respuesta;
                }

                if (string.IsNullOrEmpty(resp))
                {
                    respuesta.Add("Error", "lbErrorComunicacion");
                    return respuesta;
                }

                // --- CAMBIO CLAVE: Llamar a Replace solo si es una respuesta exitosa ---
                // Esto previene el error si el servidor devuelve HTML de error.
                resp = Replace(resp);

                respuesta = JsonConversor.ConvertirAObjeto(resp);
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.ToString();
                return respuesta;
            }
        }

        private async Task<Dictionary<string, object>> Llave(Dictionary<string, object> datos)
        {
            var respuesta = new Dictionary<string, object>();
            try
            {
                var url = datos["UrlLlave"].ToString();
                var temp = new Dictionary<string, object>();

                // Usar un usuario existente de la tabla Usuarios para solicitar la llave.
                // En tu script de BD existe el usuario ('admin','admin'), lo usamos aquí.
                temp["Entidad"] = new Usuarios
                {
                    Email = "admin",
                    Contraseña = "admin"
                };

                var stringData = JsonConversor.ConvertirAString(temp);

                var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 1, 0);
                var mensaje = await httpClient.PostAsync(url, new StringContent(stringData));
                if (!mensaje.IsSuccessStatusCode)
                {
                    respuesta.Add("Error", "lbErrorComunicacion");
                    return respuesta;
                }

                var resp = await mensaje.Content.ReadAsStringAsync();
                httpClient.Dispose(); httpClient = null;
                if (string.IsNullOrEmpty(resp))
                {
                    respuesta.Add("Error", "lbErrorComunicacion");
                    return respuesta;
                }

                resp = Replace(resp);
                respuesta = JsonConversor.ConvertirAObjeto(resp);

                // proteger contra KeyNotFoundException
                if (!respuesta.ContainsKey("Llave") || string.IsNullOrEmpty(respuesta["Llave"]?.ToString()))
                {
                    respuesta.Clear();
                    respuesta.Add("Error", "lbNoAutenticacion");
                    return respuesta;
                }

                llave = respuesta["Llave"].ToString();
                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.ToString();
                return respuesta;
            }
        }

        private string Replace(string resp)
        {
            // Solo elimina los caracteres de retorno de carro y nueva línea
            // que a veces se serializan doblemente en algunas configuraciones.
            return resp.Replace("\\\\r\\\\n", "")
                       .Replace("\\r\\n", "")
                       .Replace("\\n", "")
                       .Replace("\\r", "");
        }
    }
}