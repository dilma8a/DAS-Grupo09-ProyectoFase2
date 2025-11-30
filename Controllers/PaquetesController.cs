using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAS_Grupo09_ProyectoFase2.Models;
using DAS_Grupo09_ProyectoFase2.Services;

namespace DAS_Grupo09_ProyectoFase2.Controllers
{
    public class PaquetesController : Controller
    {
        private readonly IPaqueteService _paqueteService;
        private readonly IClienteService _clienteService;
        private readonly ILogger<PaquetesController> _logger;

        public PaquetesController(IPaqueteService paqueteService, IClienteService clienteService, ILogger<PaquetesController> logger)
        {
            _paqueteService = paqueteService;
            _clienteService = clienteService;
            _logger = logger;
        }

        private bool UsuarioNoAutenticado()
        {
            return string.IsNullOrEmpty(HttpContext.Session.GetString("Token"));
        }

        // GET: Paquetes
        public async Task<IActionResult> Index()
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            var paquetes = await _paqueteService.ObtenerTodosAsync();
            return View(paquetes);
        }

        // GET: Paquetes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return NotFound();
            }

            var paquete = await _paqueteService.ObtenerPorIdAsync(id.Value);
            if (paquete == null)
            {
                return NotFound();
            }

            return View(paquete);
        }

        // GET: Paquetes/Create
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation("=== GET Create - Cargando vista ===");

            if (UsuarioNoAutenticado())
            {
                _logger.LogWarning("Usuario no autenticado, redirigiendo a Login");
                return RedirectToAction("Index", "Login");
            }

            var clientes = await _clienteService.GetClientesAsync();
            _logger.LogInformation($"Clientes obtenidos: {clientes?.Count ?? 0}");

            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre");
            return View();
        }

        // POST: Paquetes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paquete paquete)
        {
            _logger.LogInformation("=== POST Create - Iniciando creación de paquete ===");
            _logger.LogInformation($"CodigoBarra: {paquete.CodigoBarra}");
            _logger.LogInformation($"Peso: {paquete.Peso}");
            _logger.LogInformation($"IdCliente: {paquete.IdCliente}");

            if (UsuarioNoAutenticado())
            {
                _logger.LogWarning("Usuario no autenticado");
                return RedirectToAction("Index", "Login");
            }

            // Verificar token
            var token = HttpContext.Session.GetString("Token");
            _logger.LogInformation($"Token presente: {!string.IsNullOrEmpty(token)}");
            if (!string.IsNullOrEmpty(token))
            {
                _logger.LogInformation($"Token (primeros 20 caracteres): {token.Substring(0, Math.Min(20, token.Length))}...");
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("ModelState es válido, procediendo a crear...");

                paquete.FechaRegistro = DateTime.Now;
                paquete.EstadoActual = "Recibido";

                _logger.LogInformation("Llamando a _paqueteService.CrearAsync...");

                try
                {
                    var resultado = await _paqueteService.CrearAsync(paquete);
                    _logger.LogInformation($"Resultado de CrearAsync: {resultado}");

                    if (resultado)
                    {
                        _logger.LogInformation("✅ Paquete creado exitosamente, redirigiendo a Index");
                        TempData["SuccessMessage"] = "Paquete creado exitosamente";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogError("❌ CrearAsync retornó false");
                        ModelState.AddModelError("", "Error al crear el paquete. Verifique los datos.");
                        TempData["ErrorMessage"] = "Error al crear el paquete";
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ EXCEPCIÓN al crear paquete");
                    ModelState.AddModelError("", $"Error: {ex.Message}");
                    TempData["ErrorMessage"] = $"Error: {ex.Message}";
                }
            }
            else
            {
                _logger.LogWarning("❌ ModelState NO es válido");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    if (errors.Any())
                    {
                        foreach (var error in errors)
                        {
                            _logger.LogWarning($"Error en {key}: {error.ErrorMessage}");
                        }
                    }
                }
            }

            _logger.LogInformation("Recargando vista con errores...");
            var clientes = await _clienteService.GetClientesAsync();
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre", paquete.IdCliente);
            return View(paquete);
        }

        // GET: Paquetes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return NotFound();
            }

            var paquete = await _paqueteService.ObtenerPorIdAsync(id.Value);
            if (paquete == null)
            {
                return NotFound();
            }

            var clientes = await _clienteService.GetClientesAsync();
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre", paquete.IdCliente);
            return View(paquete);
        }

        // POST: Paquetes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Paquete paquete)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id != paquete.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var resultado = await _paqueteService.ActualizarAsync(paquete);
                if (resultado)
                {
                    TempData["SuccessMessage"] = "Paquete actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al actualizar el paquete");
                }
            }

            var clientes = await _clienteService.GetClientesAsync();
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre", paquete.IdCliente);
            return View(paquete);
        }

        // GET: Paquetes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return NotFound();
            }

            var paquete = await _paqueteService.ObtenerPorIdAsync(id.Value);
            if (paquete == null)
            {
                return NotFound();
            }

            return View(paquete);
        }

        // POST: Paquetes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            var resultado = await _paqueteService.EliminarAsync(id);
            if (resultado)
            {
                TempData["SuccessMessage"] = "Paquete eliminado exitosamente";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = "Error al eliminar el paquete";
            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}