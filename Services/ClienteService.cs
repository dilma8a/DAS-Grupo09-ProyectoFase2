using DAS_Grupo09_ProyectoFase2.Models;
using System.Text;
using System.Text.Json;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public class ClienteService : IClienteService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(HttpClient httpClient, ILogger<ClienteService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Cliente>> GetClientesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Clientes");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var clientes = JsonSerializer.Deserialize<List<Cliente>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return clientes ?? new List<Cliente>();
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