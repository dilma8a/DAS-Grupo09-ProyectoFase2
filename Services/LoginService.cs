using DAS_Grupo09_ProyectoFase2.Models;
using System.Text;
using System.Text.Json;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LoginService> _logger;

        public LoginService(HttpClient httpClient, ILogger<LoginService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<LoginResponse> LoginAsync(string usuario, string clave)
        {
            try
            {
                _logger.LogInformation("=== INICIANDO LOGIN ===");
                _logger.LogInformation($"Usuario: {usuario}");
                _logger.LogInformation($"URL Base del API: {_httpClient.BaseAddress}");

                var loginRequest = new LoginRequest
                {
                    usuario = usuario,
                    clave = clave
                };

                var jsonContent = JsonSerializer.Serialize(loginRequest);
                _logger.LogInformation($"JSON a enviar: {jsonContent}");

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var url = "api/seguridad/login";
                _logger.LogInformation($"URL completa: {_httpClient.BaseAddress}{url}");

                var response = await _httpClient.PostAsync(url, content);

                _logger.LogInformation($"Status Code: {response.StatusCode}");

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Respuesta del API: {responseContent}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error HTTP {response.StatusCode}: {responseContent}");
                    return new LoginResponse
                    {
                        code = -1,
                        msj = $"Error HTTP {response.StatusCode}: No se pudo conectar al servidor"
                    };
                }

                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (loginResponse == null)
                {
                    _logger.LogError("La respuesta del API es nula");
                    return new LoginResponse
                    {
                        code = -1,
                        msj = "Error al procesar la respuesta del servidor"
                    };
                }

                _logger.LogInformation($"Login exitoso. Code: {loginResponse.code}");
                return loginResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de conexión HTTP");
                return new LoginResponse
                {
                    code = -1,
                    msj = $"Error de conexión: {ex.Message}. Verifica que el API esté ejecutándose."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en LoginAsync");
                return new LoginResponse
                {
                    code = -1,
                    msj = $"Error: {ex.Message}"
                };
            }
        }
    }
}