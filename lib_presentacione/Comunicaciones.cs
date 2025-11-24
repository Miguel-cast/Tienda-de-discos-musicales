using lib_dominio.Nucleo;
using System.Text; // NECESARIO PARA ENCODING

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
                // 1. Obtener Token
                respuesta = await Llave(datos);
                if (respuesta == null || respuesta.ContainsKey("Error"))
                    return respuesta!;

                if (string.IsNullOrEmpty(llave))
                {
                    respuesta.Add("Error", "lbErrorComunicacion: No se recibió llave");
                    return respuesta;
                }

                respuesta.Clear();

                // 2. Preparar llamada real
                var url = datos["Url"].ToString();
                datos.Remove("Url");
                datos.Remove("UrlLlave");
                datos["Llave"] = llave!; // Inyectar llave

                var stringData = JsonConversor.ConvertirAString(datos);

                var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 4, 0);

                // CORRECCIÓN 1: Definir que es application/json
                var content = new StringContent(stringData, Encoding.UTF8, "application/json");

                var message = await httpClient.PostAsync(url, content);

                if (!message.IsSuccessStatusCode)
                {
                    respuesta.Add("Error", $"Error HTTP: {message.StatusCode}");
                    return respuesta;
                }

                var resp = await message.Content.ReadAsStringAsync();
                httpClient.Dispose();

                if (string.IsNullOrEmpty(resp))
                {
                    respuesta.Add("Error", "lbErrorComunicacion: Respuesta vacía");
                    return respuesta;
                }

                //resp = Replace(resp);
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

                // CORRECCIÓN 2: Cambiar "Nombre" por "Email" para coincidir con la API
                temp["Entidad"] = new Dictionary<string, object>()
                {
                    { "Email", "admin" },
                    { "Contraseña", "admin" }
                };

                var stringData = JsonConversor.ConvertirAString(temp);

                var httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 1, 0);

                // CORRECCIÓN 3: Header JSON correcto
                var content = new StringContent(stringData, Encoding.UTF8, "application/json");

                var mensaje = await httpClient.PostAsync(url, content);

                if (!mensaje.IsSuccessStatusCode)
                {
                    respuesta.Add("Error", "lbErrorComunicacion: Fallo al obtener Token");
                    return respuesta;
                }

                var resp = await mensaje.Content.ReadAsStringAsync();
                httpClient.Dispose();

                if (string.IsNullOrEmpty(resp))
                {
                    respuesta.Add("Error", "lbErrorComunicacion");
                    return respuesta;
                }

                //resp = Replace(resp);
                respuesta = JsonConversor.ConvertirAObjeto(resp);

                if (respuesta.ContainsKey("Llave"))
                    llave = respuesta["Llave"].ToString();

                return respuesta;
            }
            catch (Exception ex)
            {
                respuesta["Error"] = ex.ToString();
                return respuesta;
            }
        }

       /* private string Replace(string resp)
        {
            // OJO: He eliminado el .Replace(" ", "") de tu código original
            // porque eso elimina los espacios en los nombres de los discos (Ej: "Pink Floyd" -> "PinkFloyd")
            return resp.Replace("\\\\r\\\\n", "")
            .Replace("\\r\\n", "")
            .Replace("\\", "")
            .Replace("\\\"", "\"")
            .Replace("\"", "'")
            .Replace("'[", "[")
            .Replace("]'", "]")
            .Replace("'{'", "{'")
            .Replace("\\\\", "\\")
            .Replace("'}'", "'}")
            .Replace("}'", "}")
            .Replace("\\n", "")
            .Replace("\\r", "")
            //.Replace(" ", "")  <-- ESTO ROMPE LOS TÍTULOS CON ESPACIOS
            .Replace("'{", "{")
            .Replace("\"", "")
            .Replace("null", "''"); */
        }
    }
