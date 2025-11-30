using DAS_Grupo09_ProyectoFase2.Models;
using System.Text.Json;
using System.Text;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public interface IEnvioService
    {
        Task<List<Envio>> ObtenerTodosAsync();
        Task<Envio> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(Envio envio);
        Task<bool> ActualizarAsync(Envio envio);
        Task<bool> EliminarAsync(int id);
    }

    public class EnvioService : IEnvioService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<EnvioService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EnvioService(HttpClient httpClient, ILogger<EnvioService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            }
        }

        public async Task<List<Envio>> ObtenerTodosAsync()
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync("api/Envios");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var envios = JsonSerializer.Deserialize<List<Envio>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return envios ?? new List<Envio>();
                }

                _logger.LogWarning($"Error al obtener envíos. Status: {response.StatusCode}");
                return new List<Envio>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener envíos");
                return new List<Envio>();
            }
        }

        public async Task<Envio> ObtenerPorIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"api/Envios/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var envio = JsonSerializer.Deserialize<Envio>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return envio;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener envío {id}");
                return null;
            }
        }

        public async Task<bool> CrearAsync(Envio envio)
        {
            try
            {
                AddAuthorizationHeader();
                var jsonContent = JsonSerializer.Serialize(envio);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Envios", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear envío");
                return false;
            }
        }

        public async Task<bool> ActualizarAsync(Envio envio)
        {
            try
            {
                AddAuthorizationHeader();
                var jsonContent = JsonSerializer.Serialize(envio);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/Envios/{envio.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar envío {envio.Id}");
                return false;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.DeleteAsync($"api/Envios/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar envío {id}");
                return false;
            }
        }
    }
}