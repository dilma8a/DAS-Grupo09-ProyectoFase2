using DAS_Grupo09_ProyectoFase2.Models;
using System.Net.Http.Json;

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

        public ReporteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EstadisticasGenerales> GetEstadisticasGeneralesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Reportes/estadisticas-generales");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<EstadisticasGenerales>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener estadísticas generales: {ex.Message}");
            }
        }

        public async Task<List<ReporteEnvio>> GetReporteEnviosAsync(DateTime? fechaInicio, DateTime? fechaFin, string? estado)
        {
            try
            {
                var query = $"api/Reportes/envios?";
                if (fechaInicio.HasValue)
                    query += $"fechaInicio={fechaInicio.Value:yyyy-MM-dd}&";
                if (fechaFin.HasValue)
                    query += $"fechaFin={fechaFin.Value:yyyy-MM-dd}&";
                if (!string.IsNullOrEmpty(estado))
                    query += $"estado={estado}&";

                var response = await _httpClient.GetAsync(query.TrimEnd('&'));
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<ReporteEnvio>>() ?? new List<ReporteEnvio>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener reporte de envíos: {ex.Message}");
            }
        }

        public async Task<List<ReporteReclamo>> GetReporteReclamosAsync(DateTime? fechaInicio, DateTime? fechaFin, string? estado)
        {
            try
            {
                var query = $"api/Reportes/reclamos?";
                if (fechaInicio.HasValue)
                    query += $"fechaInicio={fechaInicio.Value:yyyy-MM-dd}&";
                if (fechaFin.HasValue)
                    query += $"fechaFin={fechaFin.Value:yyyy-MM-dd}&";
                if (!string.IsNullOrEmpty(estado))
                    query += $"estado={estado}&";

                var response = await _httpClient.GetAsync(query.TrimEnd('&'));
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<ReporteReclamo>>() ?? new List<ReporteReclamo>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener reporte de reclamos: {ex.Message}");
            }
        }

        public async Task<List<EstadisticaPorEstado>> GetEstadisticasEnviosPorEstadoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Reportes/envios-por-estado");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<EstadisticaPorEstado>>() ?? new List<EstadisticaPorEstado>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener estadísticas por estado: {ex.Message}");
            }
        }

        public async Task<List<EstadisticaPorEstado>> GetEstadisticasReclamosPorEstadoAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Reportes/reclamos-por-estado");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<List<EstadisticaPorEstado>>() ?? new List<EstadisticaPorEstado>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener estadísticas de reclamos: {ex.Message}");
            }
        }
    }
}