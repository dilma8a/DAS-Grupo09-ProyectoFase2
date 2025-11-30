using Microsoft.AspNetCore.Mvc;
using DAS_Grupo09_ProyectoFase2.Models;
using DAS_Grupo09_ProyectoFase2.Services;

namespace DAS_Grupo09_ProyectoFase2.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(IClienteService clienteService, ILogger<ClientesController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            // Verificar autenticación
            var token = HttpContext.Session.GetString("Token");
            _logger.LogInformation("=== DEBUG CLIENTES ===");
            _logger.LogInformation($"Token en sesión: {(string.IsNullOrEmpty(token) ? "NO EXISTE" : "SÍ EXISTE")}");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("No hay token, redirigiendo a Login");
                return RedirectToAction("Index", "Login");
            }

            try
            {
                _logger.LogInformation("Llamando a GetClientesAsync...");
                var clientes = await _clienteService.GetClientesAsync();

                _logger.LogInformation($"Clientes obtenidos: {clientes?.Count ?? 0}");

                if (clientes == null || !clientes.Any())
                {
                    _logger.LogWarning("No se obtuvieron clientes del servicio");
                    TempData["ErrorMessage"] = "No se pudieron cargar los clientes. Verifica que el API esté ejecutándose.";
                    return View(new List<Cliente>());
                }

                return View(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes");
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return View(new List<Cliente>());
            }
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("Index", "Login");
            }

            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("Index", "Login");
            }

            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("Index", "Login");
            }

            if (ModelState.IsValid)
            {
                var result = await _clienteService.CreateClienteAsync(cliente);
                if (result != null)
                {
                    TempData["SuccessMessage"] = "Cliente creado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear el cliente. Verifique que el DUI y el email no estén duplicados.");
                }
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("Index", "Login");
            }

            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("Index", "Login");
            }

            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var result = await _clienteService.UpdateClienteAsync(id, cliente);
                if (result)
                {
                    TempData["SuccessMessage"] = "Cliente actualizado exitosamente";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al actualizar el cliente. Verifique que el DUI y el email no estén duplicados.");
                }
            }

            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("Index", "Login");
            }

            var cliente = await _clienteService.GetClienteByIdAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("Token")))
            {
                return RedirectToAction("Index", "Login");
            }

            var result = await _clienteService.DeleteClienteAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Cliente eliminado exitosamente";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el cliente";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}