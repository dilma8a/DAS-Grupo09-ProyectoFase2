using DAS_Grupo09_ProyectoFase2.Models;

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
        private readonly string _apiUrl = "https://localhost:7218/api/Reclamos";

        public ReclamoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Reclamo>> ObtenerTodosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Reclamo>>();
                }
                return new List<Reclamo>();
            }
            catch
            {
                return new List<Reclamo>();
            }
        }

        public async Task<Reclamo> ObtenerPorIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Reclamo>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CrearAsync(Reclamo reclamo)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiUrl, reclamo);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActualizarAsync(Reclamo reclamo)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{reclamo.Id}", reclamo);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiUrl}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Reclamo>> ObtenerPorClienteAsync(int idCliente)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/cliente/{idCliente}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Reclamo>>();
                }
                return new List<Reclamo>();
            }
            catch
            {
                return new List<Reclamo>();
            }
        }

        public async Task<List<Reclamo>> ObtenerPorEstadoAsync(string estado)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/estado/{estado}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Reclamo>>();
                }
                return new List<Reclamo>();
            }
            catch
            {
                return new List<Reclamo>();
            }
        }
    }
}
