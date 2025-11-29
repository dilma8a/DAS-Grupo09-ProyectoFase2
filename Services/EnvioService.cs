using DAS_Grupo09_ProyectoFase2.Models;

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
        private readonly string _apiUrl = "https://localhost:7139/api/Envios";

        public EnvioService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Envio>> ObtenerTodosAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<List<Envio>>();
                }
                return new List<Envio>();
            }
            catch
            {
                return new List<Envio>();
            }
        }

        public async Task<Envio> ObtenerPorIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiUrl}/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Envio>();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> CrearAsync(Envio envio)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_apiUrl, envio);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ActualizarAsync(Envio envio)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{_apiUrl}/{envio.Id}", envio);
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
