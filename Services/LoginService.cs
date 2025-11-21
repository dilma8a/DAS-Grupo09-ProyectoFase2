using DAS_Grupo09_ProyectoFase2.Models;
using System.Text;
using System.Text.Json;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public class LoginService : ILoginService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public LoginService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<LoginResponse> LoginAsync(string usuario, string clave)
        {
            try
            {
                var loginRequest = new LoginRequest
                {
                    usuario = usuario,
                    clave = clave
                };

                var jsonContent = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Seguridad/login", content);
                var responseContent = await response.Content.ReadAsStringAsync();

                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return loginResponse ?? new LoginResponse { code = -1, msj = "Error al procesar la respuesta" };
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
