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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReclamoService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Reclamo>> ObtenerTodosAsync()
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync("https://localhost:7218/api/Reclamos");
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
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"https://localhost:7218/api/Reclamos/{id}");
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
                AddAuthorizationHeader();

                var reclamoDto = new
                {
                    id = reclamo.Id,
                    idEnvio = reclamo.IdEnvio,
                    idCliente = reclamo.IdCliente,
                    tipoReclamo = reclamo.TipoReclamo,
                    descripcion = reclamo.Descripcion,
                    estado = reclamo.Estado ?? "Pendiente",
                    fechaReclamo = reclamo.FechaReclamo,
                    fechaResolucion = reclamo.FechaResolucion,
                    respuesta = reclamo.Respuesta
                };

                var response = await _httpClient.PostAsJsonAsync("https://localhost:7218/api/Reclamos", reclamoDto);
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
                AddAuthorizationHeader();

                var reclamoDto = new
                {
                    id = reclamo.Id,
                    idEnvio = reclamo.IdEnvio,
                    idCliente = reclamo.IdCliente,
                    tipoReclamo = reclamo.TipoReclamo,
                    descripcion = reclamo.Descripcion,
                    estado = reclamo.Estado ?? "Pendiente",
                    fechaReclamo = reclamo.FechaReclamo,
                    fechaResolucion = reclamo.FechaResolucion,
                    respuesta = reclamo.Respuesta
                };

                var response = await _httpClient.PutAsJsonAsync($"https://localhost:7218/api/Reclamos/{reclamo.Id}", reclamoDto);
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
                var response = await _httpClient.DeleteAsync($"https://localhost:7218/api/Reclamos/{id}");
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
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"https://localhost:7218/api/Reclamos/cliente/{idCliente}");
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
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"https://localhost:7218/api/Reclamos/estado/{estado}");
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