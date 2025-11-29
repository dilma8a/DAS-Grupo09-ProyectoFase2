using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAS_Grupo09_ProyectoFase2.Models;
using DAS_Grupo09_ProyectoFase2.Services;

namespace DAS_Grupo09_ProyectoFase2.Controllers
{
    public class ReclamosController : Controller
    {
        private readonly IReclamoService _reclamoService;
        private readonly IEnvioService _envioService;
        private readonly IClienteService _clienteService;

        public ReclamosController(IReclamoService reclamoService, IEnvioService envioService, IClienteService clienteService)
        {
            _reclamoService = reclamoService;
            _envioService = envioService;
            _clienteService = clienteService;
        }

        private bool UsuarioNoAutenticado()
        {
            return string.IsNullOrEmpty(HttpContext.Session.GetString("Token"));
        }

        // GET: Reclamos
        public async Task<IActionResult> Index()
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            var reclamos = await _reclamoService.ObtenerTodosAsync();
            return View(reclamos);
        }

        // GET: Reclamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return NotFound();
            }

            var reclamo = await _reclamoService.ObtenerPorIdAsync(id.Value);
            if (reclamo == null)
            {
                return NotFound();
            }

            return View(reclamo);
        }

        // GET: Reclamos/Create
        public async Task<IActionResult> Create()
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            var envios = await _envioService.ObtenerTodosAsync();
            var clientes = await _clienteService.GetClientesAsync();

            ViewBag.Envios = new SelectList(envios, "Id", "Id");
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre");
            ViewBag.TiposReclamo = new SelectList(new[]
            {
                "Retraso en entrega",
                "Paquete dañado",
                "Paquete perdido",
                "Dirección incorrecta",
                "Otro"
            });

            return View();
        }

        // POST: Reclamos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reclamo reclamo)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                reclamo.FechaReclamo = DateTime.Now;
                reclamo.Estado = "Pendiente";

                var resultado = await _reclamoService.CrearAsync(reclamo);
                if (resultado)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear el reclamo");
                }
            }

            var envios = await _envioService.ObtenerTodosAsync();
            var clientes = await _clienteService.GetClientesAsync();

            ViewBag.Envios = new SelectList(envios, "Id", "Id", reclamo.IdEnvio);
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre", reclamo.IdCliente);
            ViewBag.TiposReclamo = new SelectList(new[]
            {
                "Retraso en entrega",
                "Paquete dañado",
                "Paquete perdido",
                "Dirección incorrecta",
                "Otro"
            }, reclamo.TipoReclamo);

            return View(reclamo);
        }

        // GET: Reclamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return NotFound();
            }

            var reclamo = await _reclamoService.ObtenerPorIdAsync(id.Value);
            if (reclamo == null)
            {
                return NotFound();
            }

            var envios = await _envioService.ObtenerTodosAsync();
            var clientes = await _clienteService.GetClientesAsync();

            ViewBag.Envios = new SelectList(envios, "Id", "Id", reclamo.IdEnvio);
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre", reclamo.IdCliente);
            ViewBag.TiposReclamo = new SelectList(new[]
            {
                "Retraso en entrega",
                "Paquete dañado",
                "Paquete perdido",
                "Dirección incorrecta",
                "Otro"
            }, reclamo.TipoReclamo);
            ViewBag.Estados = new SelectList(new[] { "Pendiente", "En revisión", "Resuelto", "Rechazado" }, reclamo.Estado);

            return View(reclamo);
        }

        // POST: Reclamos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reclamo reclamo)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id != reclamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (reclamo.Estado == "Resuelto" && reclamo.FechaResolucion == null)
                {
                    reclamo.FechaResolucion = DateTime.Now;
                }

                var resultado = await _reclamoService.ActualizarAsync(reclamo);
                if (resultado)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al actualizar el reclamo");
                }
            }

            var envios = await _envioService.ObtenerTodosAsync();
            var clientes = await _clienteService.GetClientesAsync();

            ViewBag.Envios = new SelectList(envios, "Id", "Id", reclamo.IdEnvio);
            ViewBag.Clientes = new SelectList(clientes, "Id", "Nombre", reclamo.IdCliente);
            ViewBag.TiposReclamo = new SelectList(new[]
            {
                "Retraso en entrega",
                "Paquete dañado",
                "Paquete perdido",
                "Dirección incorrecta",
                "Otro"
            }, reclamo.TipoReclamo);
            ViewBag.Estados = new SelectList(new[] { "Pendiente", "En revisión", "Resuelto", "Rechazado" }, reclamo.Estado);

            return View(reclamo);
        }

        // GET: Reclamos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return NotFound();
            }

            var reclamo = await _reclamoService.ObtenerPorIdAsync(id.Value);
            if (reclamo == null)
            {
                return NotFound();
            }

            return View(reclamo);
        }

        // POST: Reclamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            var resultado = await _reclamoService.EliminarAsync(id);
            if (resultado)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}
