using DAS_Grupo09_ProyectoFase2.Models;

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
        private readonly string _apiUrl = "https://localhost:7139/api/Paquetes";

        public PaqueteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Paquete>> ObtenerTodosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Paquete>>();
                }
                return new List<Paquete>();
            }
            catch
            {
                return new List<Paquete>();
            }
        }

        public async Task<Paquete> ObtenerPorIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Paquete>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Paquete> ObtenerPorCodigoAsync(string codigo)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/codigo/{codigo}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Paquete>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CrearAsync(Paquete paquete)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiUrl, paquete);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActualizarAsync(Paquete paquete)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{paquete.Id}", paquete);
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
    }
}
