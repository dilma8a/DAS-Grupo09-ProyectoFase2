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

        public PaquetesController(IPaqueteService paqueteService, IClienteService clienteService)
        {
            _paqueteService = paqueteService;
            _clienteService = clienteService;
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
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}
