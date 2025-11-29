using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using DAS_Grupo09_ProyectoFase2Rest.Models;

namespace DAS_Grupo09_ProyectoFase2Rest.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaquetesController : ControllerBase
    {
        private readonly string _connectionString;

        public PaquetesController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: api/Paquetes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paquete>>> GetPaquetes()
        {
            var paquetes = new List<Paquete>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT p.*, c.Nombre as ClienteNombre 
                             FROM Paquetes p 
                             LEFT JOIN Clientes c ON p.IdCliente = c.Id 
                             ORDER BY p.FechaRegistro DESC";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        paquetes.Add(new Paquete
                        {
                            Id = reader.GetInt32(0),
                            CodigoBarra = reader.GetString(1),
                            Peso = reader.GetDecimal(2),
                            Dimensiones = reader.IsDBNull(3) ? null : reader.GetString(3),
                            Descripcion = reader.IsDBNull(4) ? null : reader.GetString(4),
                            EstadoActual = reader.IsDBNull(5) ? null : reader.GetString(5),
                            FechaRegistro = reader.GetDateTime(6),
                            IdCliente = reader.GetInt32(7),
                            Cliente = new Cliente { Nombre = reader.IsDBNull(8) ? "" : reader.GetString(8) }
                        });
                    }
                }
            }

            return Ok(paquetes);
        }

        // GET: api/Paquetes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Paquete>> GetPaquete(int id)
        {
            Paquete paquete = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT p.*, c.Nombre as ClienteNombre 
                             FROM Paquetes p 
                             LEFT JOIN Clientes c ON p.IdCliente = c.Id 
                             WHERE p.Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            paquete = new Paquete
                            {
                                Id = reader.GetInt32(0),
                                CodigoBarra = reader.GetString(1),
                                Peso = reader.GetDecimal(2),
                                Dimensiones = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Descripcion = reader.IsDBNull(4) ? null : reader.GetString(4),
                                EstadoActual = reader.IsDBNull(5) ? null : reader.GetString(5),
                                FechaRegistro = reader.GetDateTime(6),
                                IdCliente = reader.GetInt32(7),
                                Cliente = new Cliente { Nombre = reader.IsDBNull(8) ? "" : reader.GetString(8) }
                            };
                        }
                    }
                }
            }

            if (paquete == null)
                return NotFound();

            return Ok(paquete);
        }

        // GET: api/Paquetes/codigo/{codigo}
        [HttpGet("codigo/{codigo}")]
        public async Task<ActionResult<Paquete>> GetPaquetePorCodigo(string codigo)
        {
            Paquete paquete = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"SELECT p.*, c.Nombre as ClienteNombre 
                             FROM Paquetes p 
                             LEFT JOIN Clientes c ON p.IdCliente = c.Id 
                             WHERE p.CodigoBarra = @Codigo";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Codigo", codigo);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            paquete = new Paquete
                            {
                                Id = reader.GetInt32(0),
                                CodigoBarra = reader.GetString(1),
                                Peso = reader.GetDecimal(2),
                                Dimensiones = reader.IsDBNull(3) ? null : reader.GetString(3),
                                Descripcion = reader.IsDBNull(4) ? null : reader.GetString(4),
                                EstadoActual = reader.IsDBNull(5) ? null : reader.GetString(5),
                                FechaRegistro = reader.GetDateTime(6),
                                IdCliente = reader.GetInt32(7),
                                Cliente = new Cliente { Nombre = reader.IsDBNull(8) ? "" : reader.GetString(8) }
                            };
                        }
                    }
                }
            }

            if (paquete == null)
                return NotFound();

            return Ok(paquete);
        }

        // POST: api/Paquetes
        [HttpPost]
        public async Task<ActionResult<Paquete>> PostPaquete(Paquete paquete)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"INSERT INTO Paquetes (CodigoBarra, Peso, Dimensiones, Descripcion, EstadoActual, FechaRegistro, IdCliente) 
                             VALUES (@CodigoBarra, @Peso, @Dimensiones, @Descripcion, @EstadoActual, @FechaRegistro, @IdCliente);
                             SELECT CAST(SCOPE_IDENTITY() as int)";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CodigoBarra", paquete.CodigoBarra);
                    command.Parameters.AddWithValue("@Peso", paquete.Peso);
                    command.Parameters.AddWithValue("@Dimensiones", (object)paquete.Dimensiones ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Descripcion", (object)paquete.Descripcion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EstadoActual", paquete.EstadoActual ?? "Recibido");
                    command.Parameters.AddWithValue("@FechaRegistro", paquete.FechaRegistro);
                    command.Parameters.AddWithValue("@IdCliente", paquete.IdCliente);

                    var id = (int)await command.ExecuteScalarAsync();
                    paquete.Id = id;
                }
            }

            return CreatedAtAction(nameof(GetPaquete), new { id = paquete.Id }, paquete);
        }

        // PUT: api/Paquetes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaquete(int id, Paquete paquete)
        {
            if (id != paquete.Id)
                return BadRequest();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = @"UPDATE Paquetes SET 
                             CodigoBarra = @CodigoBarra, 
                             Peso = @Peso, 
                             Dimensiones = @Dimensiones, 
                             Descripcion = @Descripcion, 
                             EstadoActual = @EstadoActual,
                             IdCliente = @IdCliente 
                             WHERE Id = @Id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@CodigoBarra", paquete.CodigoBarra);
                    command.Parameters.AddWithValue("@Peso", paquete.Peso);
                    command.Parameters.AddWithValue("@Dimensiones", (object)paquete.Dimensiones ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Descripcion", (object)paquete.Descripcion ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EstadoActual", paquete.EstadoActual ?? "Recibido");
                    command.Parameters.AddWithValue("@IdCliente", paquete.IdCliente);

                    await command.ExecuteNonQueryAsync();
                }
            }

            return NoContent();
        }

        // DELETE: api/Paquetes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaquete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var query = "DELETE FROM Paquetes WHERE Id = @Id";

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
