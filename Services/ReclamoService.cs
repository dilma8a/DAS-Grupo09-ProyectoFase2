using DAS_Grupo09_ProyectoFase2.Models;
using System.Text.Json;
using System.Text;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public interface IReclamoService
    {
        Task<List<Reclamo>> ObtenerTodosAsync();
        Task<Reclamo> ObtenerPorIdAsync(int id);
        Task<bool> CrearAsync(Reclamo reclamo);
        Task<bool> ActualizarAsync(Reclamo reclamo);
        Task<bool> EliminarAsync(int id);
        Task<List<Reclamo>> ObtenerPorClienteAsync(int idCliente);
        Task<List<Reclamo>> ObtenerPorEstadoAsync(string estado);
    }

    public class ReclamoService : IReclamoService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReclamoService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReclamoService(HttpClient httpClient, ILogger<ReclamoService> logger, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Reclamo>> ObtenerTodosAsync()
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync("api/Reclamos");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var reclamos = JsonSerializer.Deserialize<List<Reclamo>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return reclamos ?? new List<Reclamo>();
                }

                _logger.LogWarning($"Error al obtener reclamos. Status: {response.StatusCode}");
                return new List<Reclamo>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reclamos");
                return new List<Reclamo>();
            }
        }

        public async Task<Reclamo> ObtenerPorIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"api/Reclamos/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var reclamo = JsonSerializer.Deserialize<Reclamo>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return reclamo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener reclamo {id}");
                return null;
            }
        }

        public async Task<bool> CrearAsync(Reclamo reclamo)
        {
            try
            {
                AddAuthorizationHeader();
                var jsonContent = JsonSerializer.Serialize(reclamo);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Reclamos", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear reclamo");
                return false;
            }
        }

        public async Task<bool> ActualizarAsync(Reclamo reclamo)
        {
            try
            {
                AddAuthorizationHeader();
                var jsonContent = JsonSerializer.Serialize(reclamo);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/Reclamos/{reclamo.Id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar reclamo {reclamo.Id}");
                return false;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.DeleteAsync($"api/Reclamos/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar reclamo {id}");
                return false;
            }
        }

        public async Task<List<Reclamo>> ObtenerPorClienteAsync(int idCliente)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"api/Reclamos/cliente/{idCliente}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var reclamos = JsonSerializer.Deserialize<List<Reclamo>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return reclamos ?? new List<Reclamo>();
                }
                return new List<Reclamo>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener reclamos por cliente {idCliente}");
                return new List<Reclamo>();
            }
        }

        public async Task<List<Reclamo>> ObtenerPorEstadoAsync(string estado)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"api/Reclamos/estado/{estado}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var reclamos = JsonSerializer.Deserialize<List<Reclamo>>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return reclamos ?? new List<Reclamo>();
                }
                return new List<Reclamo>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener reclamos por estado {estado}");
                return new List<Reclamo>();
            }
        }
    }
}