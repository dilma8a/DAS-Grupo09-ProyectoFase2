using DAS_Grupo09_ProyectoFase2.Models;
using System.Text;
using System.Text.Json;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public class ClienteService : IClienteService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ClienteService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClienteService(HttpClient httpClient, ILogger<ClienteService> logger, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Cliente>> GetClientesAsync()
        {
            try
            {
                AddAuthorizationHeader();

                _logger.LogInformation("=== LLAMANDO API ===");
                _logger.LogInformation($"URL Base: {_httpClient.BaseAddress}");
                _logger.LogInformation($"URL Completa: {_httpClient.BaseAddress}api/Clientes");

                var response = await _httpClient.GetAsync("api/Clientes");

                _logger.LogInformation($"Status Code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error HTTP: {response.StatusCode} - {errorContent}");
                    return new List<Cliente>();
                }

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation($"Respuesta: {content}");

                var clientes = JsonSerializer.Deserialize<List<Cliente>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return clientes ?? new List<Cliente>();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error de conexión HTTP al API");
                return new List<Cliente>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes");
                return new List<Cliente>();
            }
        }

        public async Task<Cliente?> GetClienteByIdAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();

                var response = await _httpClient.GetAsync($"api/Clientes/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var cliente = JsonSerializer.Deserialize<Cliente>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return cliente;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener cliente {id}");
                return null;
            }
        }

        public async Task<Cliente?> CreateClienteAsync(Cliente cliente)
        {
            try
            {
                AddAuthorizationHeader();

                var jsonContent = JsonSerializer.Serialize(cliente);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("api/Clientes", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var clienteCreado = JsonSerializer.Deserialize<Cliente>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return clienteCreado;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente");
                return null;
            }
        }

        public async Task<bool> UpdateClienteAsync(int id, Cliente cliente)
        {
            try
            {
                AddAuthorizationHeader();

                var jsonContent = JsonSerializer.Serialize(cliente);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PutAsync($"api/Clientes/{id}", content);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar cliente {id}");
                return false;
            }
        }

        public async Task<bool> DeleteClienteAsync(int id)
        {
            try
            {
                AddAuthorizationHeader();

                var response = await _httpClient.DeleteAsync($"api/Clientes/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar cliente {id}");
                return false;
            }
        }
    }
}