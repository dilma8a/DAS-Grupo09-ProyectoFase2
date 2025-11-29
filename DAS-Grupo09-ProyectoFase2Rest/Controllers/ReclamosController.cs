using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DAS_Grupo09_ProyectoFase2Rest.Models;

namespace DAS_Grupo09_ProyectoFase2Rest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReclamosController : ControllerBase
    {
        private readonly string _connectionString;

        public ReclamosController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: api/Reclamos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reclamo>>> GetReclamos()
        {
            var reclamos = new List<Reclamo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT r.*, c.Nombre as ClienteNombre 
                             FROM Reclamos r 
                             LEFT JOIN Clientes c ON r.IdCliente = c.Id 
                             ORDER BY r.FechaReclamo DESC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        reclamos.Add(new Reclamo
                        {
                            Id = reader.GetInt32(0),
                            IdEnvio = reader.GetInt32(1),
                            IdCliente = reader.GetInt32(2),
                            TipoReclamo = reader.GetString(3),
                            Descripcion = reader.GetString(4),
                            Estado = reader.GetString(5),
                            FechaReclamo = reader.GetDateTime(6),
                            FechaResolucion = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                            Respuesta = reader.IsDBNull(8) ? null : reader.GetString(8),
                            Cliente = new Cliente { Nombre = reader.IsDBNull(9) ? "" : reader.GetString(9) }
                        });
                    }
                }
            }

            return Ok(reclamos);
        }

        // GET: api/Reclamos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reclamo>> GetReclamo(int id)
        {
            Reclamo reclamo = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT r.*, c.Nombre as ClienteNombre 
                             FROM Reclamos r 
                             LEFT JOIN Clientes c ON r.IdCliente = c.Id 
                             WHERE r.Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            reclamo = new Reclamo
                            {
                                Id = reader.GetInt32(0),
                                IdEnvio = reader.GetInt32(1),
                                IdCliente = reader.GetInt32(2),
                                TipoReclamo = reader.GetString(3),
                                Descripcion = reader.GetString(4),
                                Estado = reader.GetString(5),
                                FechaReclamo = reader.GetDateTime(6),
                                FechaResolucion = reader.IsDBNull(7) ? null : reader.GetDateTime(7),
                                Respuesta = reader.IsDBNull(8) ? null : reader.GetString(8),
                                Cliente = new Cliente { Nombre = reader.IsDBNull(9) ? "" : reader.GetString(9) }
                            };
                        }
                    }
                }
            }

            if (reclamo == null)
                return NotFound();

            return Ok(reclamo);
        }

        // POST: api/Reclamos
        [HttpPost]
        public async Task<ActionResult<Reclamo>> PostReclamo(Reclamo reclamo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"INSERT INTO Reclamos (IdEnvio, IdCliente, TipoReclamo, Descripcion, Estado, FechaReclamo, FechaResolucion, Respuesta) 
                             VALUES (@IdEnvio, @IdCliente, @TipoReclamo, @Descripcion, @Estado, @FechaReclamo, @FechaResolucion, @Respuesta);
                             SELECT CAST(SCOPE_IDENTITY() as int)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdEnvio", reclamo.IdEnvio);
                    command.Parameters.AddWithValue("@IdCliente", reclamo.IdCliente);
                    command.Parameters.AddWithValue("@TipoReclamo", reclamo.TipoReclamo);
                    command.Parameters.AddWithValue("@Descripcion", reclamo.Descripcion);
                    command.Parameters.AddWithValue("@Estado", reclamo.Estado ?? "Pendiente");
                    command.Parameters.AddWithValue("@FechaReclamo", reclamo.FechaReclamo);
                    command.Parameters.AddWithValue("@FechaResolucion", (object)reclamo.FechaResolucion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Respuesta", (object)reclamo.Respuesta ?? DBNull.Value);

                    var id = (int)await command.ExecuteScalarAsync();
                    reclamo.Id = id;
                }
            }

            return CreatedAtAction(nameof(GetReclamo), new { id = reclamo.Id }, reclamo);
        }

        // PUT: api/Reclamos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReclamo(int id, Reclamo reclamo)
        {
            if (id != reclamo.Id)
                return BadRequest();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"UPDATE Reclamos SET 
                             IdEnvio = @IdEnvio, 
                             IdCliente = @IdCliente, 
                             TipoReclamo = @TipoReclamo, 
                             Descripcion = @Descripcion, 
                             Estado = @Estado,
                             FechaResolucion = @FechaResolucion,
                             Respuesta = @Respuesta 
                             WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@IdEnvio", reclamo.IdEnvio);
                    command.Parameters.AddWithValue("@IdCliente", reclamo.IdCliente);
                    command.Parameters.AddWithValue("@TipoReclamo", reclamo.TipoReclamo);
                    command.Parameters.AddWithValue("@Descripcion", reclamo.Descripcion);
                    command.Parameters.AddWithValue("@Estado", reclamo.Estado ?? "Pendiente");
                    command.Parameters.AddWithValue("@FechaResolucion", (object)reclamo.FechaResolucion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Respuesta", (object)reclamo.Respuesta ?? DBNull.Value);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return NoContent();
        }

        // DELETE: api/Reclamos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReclamo(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "DELETE FROM Reclamos WHERE Id = @Id";

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
