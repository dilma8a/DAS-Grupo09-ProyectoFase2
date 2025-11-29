using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAS_Grupo09_ProyectoFase2Rest.Data;
using DAS_Grupo09_ProyectoFase2Rest.Models;

namespace DAS_Grupo09_ProyectoFase2Rest.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly EnvioPaqueteContext _context;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(EnvioPaqueteContext context, ILogger<ClientesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Obtener todos los clientes activos
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            try
            {
                var clientes = await _context.Clientes
                    .Where(c => c.EstaActivo)
                    .OrderBy(c => c.Nombre)
                    .ToListAsync();

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes");
                return StatusCode(500, new { message = "Error al obtener clientes" });
            }
        }

        /// <summary>
        /// Obtener un cliente por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null || !cliente.EstaActivo)
                {
                    return NotFound(new { message = "Cliente no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener cliente {id}");
                return StatusCode(500, new { message = "Error al obtener cliente" });
            }
        }

        /// <summary>
        /// Crear un nuevo cliente
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Cliente>> CreateCliente(Cliente cliente)
        {
            try
            {
                // Validar DUI único
                if (await _context.Clientes.AnyAsync(c => c.DUI == cliente.DUI))
                {
                    return BadRequest(new { message = "El DUI ya está registrado" });
                }

                // Validar Email único
                if (await _context.Clientes.AnyAsync(c => c.Email == cliente.Email))
                {
                    return BadRequest(new { message = "El email ya está registrado" });
                }

                cliente.FechaRegistro = DateTime.Now;
                cliente.EstaActivo = true;

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Cliente creado: {cliente.Nombre}");

                return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente");
                return StatusCode(500, new { message = "Error al crear cliente" });
            }
        }

        /// <summary>
        /// Actualizar un cliente existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest(new { message = "El ID no coincide" });
            }

            try
            {
                var clienteExistente = await _context.Clientes.FindAsync(id);
                if (clienteExistente == null || !clienteExistente.EstaActivo)
                {
                    return NotFound(new { message = "Cliente no encontrado" });
                }

                // Validar DUI único (excepto el propio cliente)
                if (await _context.Clientes.AnyAsync(c => c.DUI == cliente.DUI && c.Id != id))
                {
                    return BadRequest(new { message = "El DUI ya está registrado" });
                }

                // Validar Email único (excepto el propio cliente)
                if (await _context.Clientes.AnyAsync(c => c.Email == cliente.Email && c.Id != id))
                {
                    return BadRequest(new { message = "El email ya está registrado" });
                }

                clienteExistente.Nombre = cliente.Nombre;
                clienteExistente.DUI = cliente.DUI;
                clienteExistente.Telefono = cliente.Telefono;
                clienteExistente.Email = cliente.Email;
                clienteExistente.Direccion = cliente.Direccion;

                await _context.SaveChangesAsync();

                _logger.LogInformation($"Cliente actualizado: {cliente.Nombre}");

                return Ok(clienteExistente);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Clientes.AnyAsync(e => e.Id == id))
                {
                    return NotFound();
                }
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar cliente {id}");
                return StatusCode(500, new { message = "Error al actualizar cliente" });
            }
        }

        /// <summary>
        /// Eliminar un cliente (eliminación lógica)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null || !cliente.EstaActivo)
                {
                    return NotFound(new { message = "Cliente no encontrado" });
                }

                // Eliminación lógica
                cliente.EstaActivo = false;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Cliente eliminado: {cliente.Nombre}");

                return Ok(new { message = "Cliente eliminado correctamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar cliente {id}");
                return StatusCode(500, new { message = "Error al eliminar cliente" });
            }
        }
    }
}