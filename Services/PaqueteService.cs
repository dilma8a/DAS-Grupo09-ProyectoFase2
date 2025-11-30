using DAS_Grupo09_ProyectoFase2.Models;
using System.Text;
using System.Text.Json;

namespace DAS_Grupo09_ProyectoFase2.Services
{
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
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var paquete = JsonSerializer.Deserialize<Paquete>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return paquete;
                }
                return null;
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
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var paquete = JsonSerializer.Deserialize<Paquete>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    return paquete;
                }
                return null;
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
                _logger.LogInformation("=== CREANDO PAQUETE ===");
                _logger.LogInformation($"Codigo: {paquete.CodigoBarra}");
                _logger.LogInformation($"IdCliente: {paquete.IdCliente}");

                // ⬇️ CREAR DTO SIN LA PROPIEDAD CLIENTE
                var paqueteDto = new
                {
                    id = paquete.Id,
                    codigoBarra = paquete.CodigoBarra,
                    peso = paquete.Peso,
                    dimensiones = paquete.Dimensiones,
                    descripcion = paquete.Descripcion,
                    estadoActual = paquete.EstadoActual ?? "Recibido",
                    fechaRegistro = paquete.FechaRegistro,
                    idCliente = paquete.IdCliente
                    // ⬆️ NO incluimos "cliente" aquí
                };

                var jsonContent = JsonSerializer.Serialize(paqueteDto);
                _logger.LogInformation($"JSON a enviar: {jsonContent}");

                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Paquetes", content);

                _logger.LogInformation($"Status Code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error de API: {errorContent}");
                }

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