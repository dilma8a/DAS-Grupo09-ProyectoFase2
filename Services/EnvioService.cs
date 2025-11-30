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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EnvioService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
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
                var response = await _httpClient.GetAsync("https://localhost:7218/api/Envios");
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
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"https://localhost:7218/api/Envios/{id}");
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
                AddAuthorizationHeader();

                var envioDto = new
                {
                    id = envio.Id,
                    idPaquete = envio.IdPaquete,
                    idConductor = envio.IdConductor,
                    origen = envio.Origen,
                    destino = envio.Destino,
                    fechaSalida = envio.FechaSalida,
                    fechaEntrega = envio.FechaEntrega,
                    estadoEnvio = envio.EstadoEnvio ?? "Pendiente",
                    observaciones = envio.Observaciones
                };

                var response = await _httpClient.PostAsJsonAsync("https://localhost:7218/api/Envios", envioDto);
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
                AddAuthorizationHeader();

                var envioDto = new
                {
                    id = envio.Id,
                    idPaquete = envio.IdPaquete,
                    idConductor = envio.IdConductor,
                    origen = envio.Origen,
                    destino = envio.Destino,
                    fechaSalida = envio.FechaSalida,
                    fechaEntrega = envio.FechaEntrega,
                    estadoEnvio = envio.EstadoEnvio ?? "Pendiente",
                    observaciones = envio.Observaciones
                };

                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7218/api/Envios/{envio.Id}", envioDto);
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
                AddAuthorizationHeader();
                var response = await _httpClient.DeleteAsync($"https://localhost:7218/api/Envios/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}