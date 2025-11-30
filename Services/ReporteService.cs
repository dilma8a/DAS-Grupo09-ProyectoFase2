using DAS_Grupo09_ProyectoFase2.Models;
using System.Text.Json;

namespace DAS_Grupo09_ProyectoFase2.Services
{
    public interface IReporteService
    {
        Task<EstadisticasGenerales> GetEstadisticasGeneralesAsync();
        Task<List<ReporteEnvio>> GetReporteEnviosAsync(DateTime? fechaInicio, DateTime? fechaFin, string? estado);
        Task<List<ReporteReclamo>> GetReporteReclamosAsync(DateTime? fechaInicio, DateTime? fechaFin, string? estado);
        Task<List<EstadisticaPorEstado>> GetEstadisticasEnviosPorEstadoAsync();
        Task<List<EstadisticaPorEstado>> GetEstadisticasReclamosPorEstadoAsync();
    }

    public class ReporteService : IReporteService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ReporteService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReporteService(HttpClient httpClient, ILogger<ReporteService> logger, IHttpContextAccessor httpContextAccessor)
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
                _logger.LogInformation($"Token agregado al ReporteService: {token.Substring(0, 20)}...");
            }
            else
            {
                _logger.LogWarning("No se encontró token en sesión para ReporteService");
            }
        }

        public async Task<EstadisticasGenerales> GetEstadisticasGeneralesAsync()
        {
            try
            {
                AddAuthorizationHeader();
                _logger.LogInformation("Llamando a: api/Reportes/estadisticas-generales");

                var response = await _httpClient.GetAsync("api/Reportes/estadisticas-generales");

                _logger.LogInformation($"Status Code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error en estadísticas generales: {response.StatusCode} - {error}");
                    throw new Exception($"Error al obtener estadísticas generales: {response.StatusCode}");
                }

                var content = await response.Content.ReadAsStringAsync();
                var estadisticas = JsonSerializer.Deserialize<EstadisticasGenerales>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return estadisticas ?? new EstadisticasGenerales();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener estadísticas generales: {ex.Message}");
                throw new Exception($"Error al obtener estadísticas generales: {ex.Message}");
            }
        }

        public async Task<List<ReporteEnvio>> GetReporteEnviosAsync(DateTime? fechaInicio, DateTime? fechaFin, string? estado)
        {
            try
            {
                AddAuthorizationHeader();

                var query = "api/Reportes/envios?";
                if (fechaInicio.HasValue)
                    query += $"fechaInicio={fechaInicio.Value:yyyy-MM-dd}&";
                if (fechaFin.HasValue)
                    query += $"fechaFin={fechaFin.Value:yyyy-MM-dd}&";
                if (!string.IsNullOrEmpty(estado))
                    query += $"estado={estado}&";

                var response = await _httpClient.GetAsync(query.TrimEnd('&'));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error al obtener reporte de envíos: {response.StatusCode}");
                    return new List<ReporteEnvio>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var reportes = JsonSerializer.Deserialize<List<ReporteEnvio>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return reportes ?? new List<ReporteEnvio>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener reporte de envíos: {ex.Message}");
                throw new Exception($"Error al obtener reporte de envíos: {ex.Message}");
            }
        }

        public async Task<List<ReporteReclamo>> GetReporteReclamosAsync(DateTime? fechaInicio, DateTime? fechaFin, string? estado)
        {
            try
            {
                AddAuthorizationHeader();

                var query = "api/Reportes/reclamos?";
                if (fechaInicio.HasValue)
                    query += $"fechaInicio={fechaInicio.Value:yyyy-MM-dd}&";
                if (fechaFin.HasValue)
                    query += $"fechaFin={fechaFin.Value:yyyy-MM-dd}&";
                if (!string.IsNullOrEmpty(estado))
                    query += $"estado={estado}&";

                var response = await _httpClient.GetAsync(query.TrimEnd('&'));

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error al obtener reporte de reclamos: {response.StatusCode}");
                    return new List<ReporteReclamo>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var reportes = JsonSerializer.Deserialize<List<ReporteReclamo>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return reportes ?? new List<ReporteReclamo>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener reporte de reclamos: {ex.Message}");
                throw new Exception($"Error al obtener reporte de reclamos: {ex.Message}");
            }
        }

        public async Task<List<EstadisticaPorEstado>> GetEstadisticasEnviosPorEstadoAsync()
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync("api/Reportes/envios-por-estado");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error al obtener estadísticas por estado: {response.StatusCode}");
                    return new List<EstadisticaPorEstado>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var estadisticas = JsonSerializer.Deserialize<List<EstadisticaPorEstado>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return estadisticas ?? new List<EstadisticaPorEstado>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener estadísticas por estado: {ex.Message}");
                throw new Exception($"Error al obtener estadísticas por estado: {ex.Message}");
            }
        }

        public async Task<List<EstadisticaPorEstado>> GetEstadisticasReclamosPorEstadoAsync()
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync("api/Reportes/reclamos-por-estado");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Error al obtener estadísticas de reclamos: {response.StatusCode}");
                    return new List<EstadisticaPorEstado>();
                }

                var content = await response.Content.ReadAsStringAsync();
                var estadisticas = JsonSerializer.Deserialize<List<EstadisticaPorEstado>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return estadisticas ?? new List<EstadisticaPorEstado>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener estadísticas de reclamos: {ex.Message}");
                throw new Exception($"Error al obtener estadísticas de reclamos: {ex.Message}");
            }
        }
    }
}