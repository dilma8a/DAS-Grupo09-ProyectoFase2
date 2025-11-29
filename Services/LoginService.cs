using DAS_Grupo09_ProyectoFase2.Models;
using System.Net.Http.Json;
using System.Text.Json;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _http;

        public LoginService(HttpClient http)
        {
            _http = http;
        }

        public async Task<LoginResponse> LoginAsync(string usuario, string clave)
        {
            // Este objeto anónimo se serializa a JSON como:
            // { "Usuario": "admin", "Clave": "1234" }
            var request = new
            {
                Usuario = usuario,
                Clave = clave
            };

            try
            {
                // Llama a POST https://localhost:PUERTO/api/Seguridad/login
                var response = await _http.PostAsJsonAsync("Seguridad/login", request);

                var raw = await response.Content.ReadAsStringAsync();

                // Si la API devolvió error HTTP (404, 500, etc.)
                if (!response.IsSuccessStatusCode)
                {
                    return new LoginResponse
                    {
                        code = -1,
                        msj = $"Error HTTP {(int)response.StatusCode}: {response.ReasonPhrase}. Detalle: {raw}"
                    };
                }

                if (string.IsNullOrWhiteSpace(raw))
                {
                    return new LoginResponse
                    {
                        code = -1,
                        msj = "La API devolvió una respuesta vacía."
                    };
                }

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Mapea directamente al modelo que ya usas en tu LoginController
                var result = JsonSerializer.Deserialize<LoginResponse>(raw, options);

                if (result == null)
                {
                    return new LoginResponse
                    {
                        code = -1,
                        msj = "No se pudo interpretar la respuesta del servidor."
                    };
                }

                return result;
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    code = -1,
                    msj = $"Error al conectar con el servicio: {ex.Message}"
                };
            }
        }
    }
}
