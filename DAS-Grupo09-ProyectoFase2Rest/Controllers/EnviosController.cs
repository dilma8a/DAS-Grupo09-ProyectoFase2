using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DAS_Grupo09_ProyectoFase2Rest.Models;

namespace DAS_Grupo09_ProyectoFase2Rest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EnviosController : ControllerBase
    {
        private readonly string _connectionString;

        public EnviosController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Envio>>> GetEnvios()
        {
            var envios = new List<Envio>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT e.*, p.CodigoBarra as PaqueteCodigo 
                             FROM Envios e 
                             LEFT JOIN Paquetes p ON e.IdPaquete = p.Id 
                             ORDER BY e.Id DESC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        envios.Add(new Envio
                        {
                            Id = reader.GetInt32(0),
                            IdPaquete = reader.GetInt32(1),
                            IdConductor = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                            Origen = reader.GetString(3),
                            Destino = reader.GetString(4),
                            FechaSalida = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                            FechaEntrega = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                            EstadoEnvio = reader.GetString(7),
                            Observaciones = reader.IsDBNull(8) ? null : reader.GetString(8),
                            Paquete = new Paquete { CodigoBarra = reader.IsDBNull(9) ? "" : reader.GetString(9) }
                        });
                    }
                }
            }

            return Ok(envios);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Envio>> GetEnvio(int id)
        {
            Envio envio = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT e.*, p.CodigoBarra as PaqueteCodigo 
                             FROM Envios e 
                             LEFT JOIN Paquetes p ON e.IdPaquete = p.Id 
                             WHERE e.Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            envio = new Envio
                            {
                                Id = reader.GetInt32(0),
                                IdPaquete = reader.GetInt32(1),
                                IdConductor = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                                Origen = reader.GetString(3),
                                Destino = reader.GetString(4),
                                FechaSalida = reader.IsDBNull(5) ? null : reader.GetDateTime(5),
                                FechaEntrega = reader.IsDBNull(6) ? null : reader.GetDateTime(6),
                                EstadoEnvio = reader.GetString(7),
                                Observaciones = reader.IsDBNull(8) ? null : reader.GetString(8),
                                Paquete = new Paquete { CodigoBarra = reader.IsDBNull(9) ? "" : reader.GetString(9) }
                            };
                        }
                    }
                }
            }

            if (envio == null)
                return NotFound();

            return Ok(envio);
        }

        [HttpPost]
        public async Task<ActionResult<Envio>> PostEnvio(Envio envio)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"INSERT INTO Envios (IdPaquete, IdConductor, Origen, Destino, FechaSalida, FechaEntrega, EstadoEnvio, Observaciones) 
                             VALUES (@IdPaquete, @IdConductor, @Origen, @Destino, @FechaSalida, @FechaEntrega, @EstadoEnvio, @Observaciones);
                             SELECT CAST(SCOPE_IDENTITY() as int)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPaquete", envio.IdPaquete);
                    command.Parameters.AddWithValue("@IdConductor", (object)envio.IdConductor ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Origen", envio.Origen);
                    command.Parameters.AddWithValue("@Destino", envio.Destino);
                    command.Parameters.AddWithValue("@FechaSalida", (object)envio.FechaSalida ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FechaEntrega", (object)envio.FechaEntrega ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EstadoEnvio", envio.EstadoEnvio ?? "Pendiente");
                    command.Parameters.AddWithValue("@Observaciones", (object)envio.Observaciones ?? DBNull.Value);

                    var id = (int)await command.ExecuteScalarAsync();
                    envio.Id = id;
                }
            }

            return CreatedAtAction(nameof(GetEnvio), new { id = envio.Id }, envio);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnvio(int id, Envio envio)
        {
            if (id != envio.Id)
                return BadRequest();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"UPDATE Envios SET 
                             IdPaquete = @IdPaquete, 
                             IdConductor = @IdConductor, 
                             Origen = @Origen, 
                             Destino = @Destino, 
                             FechaSalida = @FechaSalida, 
                             FechaEntrega = @FechaEntrega, 
                             EstadoEnvio = @EstadoEnvio, 
                             Observaciones = @Observaciones 
                             WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@IdPaquete", envio.IdPaquete);
                    command.Parameters.AddWithValue("@IdConductor", (object)envio.IdConductor ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Origen", envio.Origen);
                    command.Parameters.AddWithValue("@Destino", envio.Destino);
                    command.Parameters.AddWithValue("@FechaSalida", (object)envio.FechaSalida ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FechaEntrega", (object)envio.FechaEntrega ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EstadoEnvio", envio.EstadoEnvio ?? "Pendiente");
                    command.Parameters.AddWithValue("@Observaciones", (object)envio.Observaciones ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnvio(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "DELETE FROM Envios WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected == 0)
                        return NotFound();
                }
            }

            return NoContent();
        }
    }
}
