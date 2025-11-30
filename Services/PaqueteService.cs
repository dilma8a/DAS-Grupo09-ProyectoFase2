using DAS_Grupo09_ProyectoFase2.Models;
using System.Text.Json;
using System.Text;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public interface IPaqueteService
    {
        Task<List<Paquete>> ObtenerTodosAsync();
        Task<Paquete> ObtenerPorIdAsync(int id);
        Task<Paquete> ObtenerPorCodigoAsync(string codigo);
        Task<bool> CrearAsync(Paquete paquete);
        Task<bool> ActualizarAsync(Paquete paquete);
        Task<bool> EliminarAsync(int id);
    }

    public class PaqueteService : IPaqueteService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PaqueteService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaqueteService(HttpClient httpClient, ILogger<PaqueteService> logger, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Paquete>> ObtenerTodosAsync()
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync("api/Paquetes");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var paquetes = JsonSerializer.Deserialize<List<Paquete>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return paquetes ?? new List<Paquete>();
                }

                _logger.LogWarning($"Error al obtener paquetes. Status: {response.StatusCode}");
                return new List<Paquete>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener paquetes");
                return new List<Paquete>();
            }
        }

        public async Task<Paquete> ObtenerPorIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"api/Paquetes/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var paquete = JsonSerializer.Deserialize<Paquete>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return paquete;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener paquete {id}");
                return null;
            }
        }

        public async Task<Paquete> ObtenerPorCodigoAsync(string codigo)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"api/Paquetes/codigo/{codigo}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var paquete = JsonSerializer.Deserialize<Paquete>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return paquete;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener paquete por código {codigo}");
                return null;
            }
        }

        public async Task<bool> CrearAsync(Paquete paquete)
        {
            try
            {
                AddAuthorizationHeader();
                var jsonContent = JsonSerializer.Serialize(paquete);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Paquetes", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear paquete");
                return false;
            }
        }

        public async Task<bool> ActualizarAsync(Paquete paquete)
        {
            try
            {
                AddAuthorizationHeader();
                var jsonContent = JsonSerializer.Serialize(paquete);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/Paquetes/{paquete.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar paquete {paquete.Id}");
                return false;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.DeleteAsync($"api/Paquetes/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar paquete {id}");
                return false;
            }
        }
    }
}