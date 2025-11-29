using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DAS_Grupo09_ProyectoFase2Rest.Models;

namespace DAS_Grupo09_ProyectoFase2Rest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly string _connectionString;

        public ReportesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: api/Reportes/estadisticas-generales
        [HttpGet("estadisticas-generales")]
        public async Task<ActionResult<EstadisticasGenerales>> GetEstadisticasGenerales()
        {
            var estadisticas = new EstadisticasGenerales();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                // Total de envíos
                var query = "SELECT COUNT(*) FROM Envios";
                using (var command = new SqlCommand(query, connection))
                {
                    estadisticas.TotalEnvios = (int)await command.ExecuteScalarAsync();
                }

                // Total de clientes
                query = "SELECT COUNT(*) FROM Clientes";
                using (var command = new SqlCommand(query, connection))
                {
                    estadisticas.TotalClientes = (int)await command.ExecuteScalarAsync();
                }

                // Total de paquetes
                query = "SELECT COUNT(*) FROM Paquetes";
                using (var command = new SqlCommand(query, connection))
                {
                    estadisticas.TotalPaquetes = (int)await command.ExecuteScalarAsync();
                }

                // Total de reclamos
                query = "SELECT COUNT(*) FROM Reclamos";
                using (var command = new SqlCommand(query, connection))
                {
                    estadisticas.TotalReclamos = (int)await command.ExecuteScalarAsync();
                }

                // Envíos por estado
                query = "SELECT COUNT(*) FROM Envios WHERE EstadoEnvio = 'En tránsito'";
                using (var command = new SqlCommand(query, connection))
                {
                    estadisticas.EnviosEnTransito = (int)await command.ExecuteScalarAsync();
                }

                query = "SELECT COUNT(*) FROM Envios WHERE EstadoEnvio = 'Entregado'";
                using (var command = new SqlCommand(query, connection))
                {
                    estadisticas.EnviosEntregados = (int)await command.ExecuteScalarAsync();
                }

                query = "SELECT COUNT(*) FROM Envios WHERE EstadoEnvio = 'Pendiente'";
                using (var command = new SqlCommand(query, connection))
                {
                    estadisticas.EnviosPendientes = (int)await command.ExecuteScalarAsync();
                }

                // Reclamos por estado
                query = "SELECT COUNT(*) FROM Reclamos WHERE Estado = 'Pendiente'";
                using (var command = new SqlCommand(query, connection))
                {
                    estadisticas.ReclamosPendientes = (int)await command.ExecuteScalarAsync();
                }

                query = "SELECT COUNT(*) FROM Reclamos WHERE Estado = 'Resuelto'";
                using (var command = new SqlCommand(query, connection))
                {
                    estadisticas.ReclamosResueltos = (int)await command.ExecuteScalarAsync();
                }
            }

            return Ok(estadisticas);
        }

        // GET: api/Reportes/envios
        [HttpGet("envios")]
        public async Task<ActionResult<IEnumerable<ReporteEnvio>>> GetReporteEnvios(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] string? estado)
        {
            var reportes = new List<ReporteEnvio>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"SELECT e.Id, e.CodigoSeguimiento, c.Nombre as ClienteNombre, 
                             e.Origen, e.Destino, e.EstadoEnvio, e.FechaSalida, e.FechaEntrega,
                             DATEDIFF(day, e.FechaSalida, ISNULL(e.FechaEntrega, GETDATE())) as DiasTranscurridos
                             FROM Envios e
                             INNER JOIN Paquetes p ON e.IdPaquete = p.Id
                             INNER JOIN Clientes c ON p.IdCliente = c.Id
                             WHERE 1=1";

                if (fechaInicio.HasValue)
                    query += " AND e.FechaSalida >= @FechaInicio";
                if (fechaFin.HasValue)
                    query += " AND e.FechaSalida <= @FechaFin";
                if (!string.IsNullOrEmpty(estado))
                    query += " AND e.EstadoEnvio = @Estado";

                query += " ORDER BY e.FechaSalida DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    if (fechaInicio.HasValue)
                        command.Parameters.AddWithValue("@FechaInicio", fechaInicio.Value);
                    if (fechaFin.HasValue)
                        command.Parameters.AddWithValue("@FechaFin", fechaFin.Value);
                    if (!string.IsNullOrEmpty(estado))
                        command.Parameters.AddWithValue("@Estado", estado);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reportes.Add(new ReporteEnvio
                            {
                                Id = reader.GetInt32(0),
                                CodigoSeguimiento = reader.GetString(1),
                                ClienteNombre = reader.GetString(2),
                                Origen = reader.GetString(3),
                                Destino = reader.GetString(4),
                                Estado = reader.GetString(5),
                                FechaEnvio = reader.GetDateTime(6),
                                FechaEntrega = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                                DiasTranscurridos = reader.GetInt32(8)
                            });
                        }
                    }
                }
            }

            return Ok(reportes);
        }

        // GET: api/Reportes/reclamos
        [HttpGet("reclamos")]
        public async Task<ActionResult<IEnumerable<ReporteReclamo>>> GetReporteReclamos(
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] string? estado)
        {
            var reportes = new List<ReporteReclamo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"SELECT r.Id, c.Nombre as ClienteNombre, r.IdEnvio, 
                             r.TipoReclamo, r.Estado, r.FechaReclamo, r.FechaResolucion,
                             DATEDIFF(day, r.FechaReclamo, ISNULL(r.FechaResolucion, GETDATE())) as DiasResolucion
                             FROM Reclamos r
                             INNER JOIN Clientes c ON r.IdCliente = c.Id
                             WHERE 1=1";

                if (fechaInicio.HasValue)
                    query += " AND r.FechaReclamo >= @FechaInicio";
                if (fechaFin.HasValue)
                    query += " AND r.FechaReclamo <= @FechaFin";
                if (!string.IsNullOrEmpty(estado))
                    query += " AND r.Estado = @Estado";

                query += " ORDER BY r.FechaReclamo DESC";

                using (var command = new SqlCommand(query, connection))
                {
                    if (fechaInicio.HasValue)
                        command.Parameters.AddWithValue("@FechaInicio", fechaInicio.Value);
                    if (fechaFin.HasValue)
                        command.Parameters.AddWithValue("@FechaFin", fechaFin.Value);
                    if (!string.IsNullOrEmpty(estado))
                        command.Parameters.AddWithValue("@Estado", estado);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            reportes.Add(new ReporteReclamo
                            {
                                Id = reader.GetInt32(0),
                                ClienteNombre = reader.GetString(1),
                                EnvioId = reader.GetInt32(2),
                                TipoReclamo = reader.GetString(3),
                                Estado = reader.GetString(4),
                                FechaReclamo = reader.GetDateTime(5),
                                FechaResolucion = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                                DiasResolucion = reader.GetInt32(7)
                            });
                        }
                    }
                }
            }

            return Ok(reportes);
        }

        // GET: api/Reportes/envios-por-estado
        [HttpGet("envios-por-estado")]
        public async Task<ActionResult<IEnumerable<EstadisticaPorEstado>>> GetEnviosPorEstado()
        {
            var estadisticas = new List<EstadisticaPorEstado>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"SELECT EstadoEnvio, COUNT(*) as Cantidad,
                             CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Envios) AS DECIMAL(10,2)) as Porcentaje
                             FROM Envios
                             GROUP BY EstadoEnvio
                             ORDER BY Cantidad DESC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        estadisticas.Add(new EstadisticaPorEstado
                        {
                            Estado = reader.GetString(0),
                            Cantidad = reader.GetInt32(1),
                            Porcentaje = reader.GetDecimal(2)
                        });
                    }
                }
            }

            return Ok(estadisticas);
        }

        // GET: api/Reportes/reclamos-por-estado
        [HttpGet("reclamos-por-estado")]
        public async Task<ActionResult<IEnumerable<EstadisticaPorEstado>>> GetReclamosPorEstado()
        {
            var estadisticas = new List<EstadisticaPorEstado>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var query = @"SELECT Estado, COUNT(*) as Cantidad,
                             CAST(COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Reclamos) AS DECIMAL(10,2)) as Porcentaje
                             FROM Reclamos
                             GROUP BY Estado
                             ORDER BY Cantidad DESC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        estadisticas.Add(new EstadisticaPorEstado
                        {
                            Estado = reader.GetString(0),
                            Cantidad = reader.GetInt32(1),
                            Porcentaje = reader.GetDecimal(2)
                        });
                    }
                }
            }

            return Ok(estadisticas);
        }
    }
}
