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
            _logger.LogInformation("=== ENTRANDO A PAQUETES INDEX ===");

            if (UsuarioNoAutenticado())
            {
                _logger.LogWarning("Usuario no autenticado. Redirigiendo a Login.");
                return RedirectToAction("Index", "Login");
            }

            var token = HttpContext.Session.GetString("Token");
            _logger.LogInformation($"Token encontrado: {token?.Substring(0, 20)}...");

            try
            {
                _logger.LogInformation("Llamando a _paqueteService.ObtenerTodosAsync()...");
                var paquetes = await _paqueteService.ObtenerTodosAsync();

                if (paquetes == null)
                {
                    _logger.LogError("❌ ObtenerTodosAsync() devolvió NULL");
                    TempData["ErrorMessage"] = "Error: El servicio devolvió NULL";
                    return View(new List<Paquete>());
                }

                _logger.LogInformation($"✅ Paquetes obtenidos: {paquetes.Count}");

                if (paquetes.Count == 0)
                {
                    _logger.LogWarning("⚠️ La lista de paquetes está vacía");
                    TempData["InfoMessage"] = "No hay paquetes registrados en el sistema";
                }
                else
                {
                    foreach (var p in paquetes)
                    {
                        _logger.LogInformation($"  - Paquete ID: {p.Id}, Código: {p.CodigoBarra}, Cliente: {p.IdCliente}");
                    }
                }

                return View(paquetes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ EXCEPCIÓN en Index: {ex.Message}");
                _logger.LogError($"StackTrace: {ex.StackTrace}");
                TempData["ErrorMessage"] = $"Error al cargar paquetes: {ex.Message}";
                return View(new List<Paquete>());
            }
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
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            var clientes = await _clienteService.GetClientesAsync();
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre");
            return View();
        }

        // POST: Paquetes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paquete paquete)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                paquete.FechaRegistro = DateTime.Now;
                paquete.EstadoActual = "Recibido";

                var resultado = await _paqueteService.CrearAsync(paquete);
                if (resultado)
                {
                    TempData["SuccessMessage"] = "Paquete creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear el paquete");
                }
            }

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

            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}