using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAS_Grupo09_ProyectoFase2.Models;
using DAS_Grupo09_ProyectoFase2.Services;

namespace DAS_Grupo09_ProyectoFase2.Controllers
{
    public class EnviosController : Controller
    {
        private readonly IEnvioService _envioService;
        private readonly IPaqueteService _paqueteService;

        public EnviosController(IEnvioService envioService, IPaqueteService paqueteService)
        {
            _envioService = envioService;
            _paqueteService = paqueteService;
        }

        private bool UsuarioNoAutenticado()
        {
            return string.IsNullOrEmpty(HttpContext.Session.GetString("Token"));
        }

        // GET: Envios
        public async Task<IActionResult> Index()
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            var envios = await _envioService.ObtenerTodosAsync();
            return View(envios);
        }

        // GET: Envios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return NotFound();
            }

            var envio = await _envioService.ObtenerPorIdAsync(id.Value);
            if (envio == null)
            {
                return NotFound();
            }

            return View(envio);
        }

        // GET: Envios/Create
        public async Task<IActionResult> Create()
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            var paquetes = await _paqueteService.ObtenerTodosAsync();
            ViewBag.Paquetes = new SelectList(paquetes, "Id", "CodigoBarra");
            return View();
        }

        // POST: Envios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Envio envio)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                envio.EstadoEnvio = "Pendiente";

                var resultado = await _envioService.CrearAsync(envio);
                if (resultado)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al crear el envío");
                }
            }

            var paquetes = await _paqueteService.ObtenerTodosAsync();
            ViewBag.Paquetes = new SelectList(paquetes, "Id", "CodigoBarra", envio.IdPaquete);
            return View(envio);
        }

        // GET: Envios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return NotFound();
            }

            var envio = await _envioService.ObtenerPorIdAsync(id.Value);
            if (envio == null)
            {
                return NotFound();
            }

            var paquetes = await _paqueteService.ObtenerTodosAsync();
            ViewBag.Paquetes = new SelectList(paquetes, "Id", "CodigoBarra", envio.IdPaquete);

            ViewBag.Estados = new SelectList(new[]
            {
                new { Value = "Pendiente", Text = "Pendiente" },
                new { Value = "En tránsito", Text = "En tránsito" },
                new { Value = "Entregado", Text = "Entregado" },
                new { Value = "Retrasado", Text = "Retrasado" }
            }, "Value", "Text", envio.EstadoEnvio);

            return View(envio);
        }

        // POST: Envios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Envio envio)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id != envio.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var resultado = await _envioService.ActualizarAsync(envio);
                if (resultado)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Error al actualizar el envío");
                }
            }

            var paquetes = await _paqueteService.ObtenerTodosAsync();
            ViewBag.Paquetes = new SelectList(paquetes, "Id", "CodigoBarra", envio.IdPaquete);

            ViewBag.Estados = new SelectList(new[]
            {
                new { Value = "Pendiente", Text = "Pendiente" },
                new { Value = "En tránsito", Text = "En tránsito" },
                new { Value = "Entregado", Text = "Entregado" },
                new { Value = "Retrasado", Text = "Retrasado" }
            }, "Value", "Text", envio.EstadoEnvio);

            return View(envio);
        }

        // GET: Envios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return NotFound();
            }

            var envio = await _envioService.ObtenerPorIdAsync(id.Value);
            if (envio == null)
            {
                return NotFound();
            }

            return View(envio);
        }

        // POST: Envios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            var resultado = await _envioService.EliminarAsync(id);
            if (resultado)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Delete), new { id });
        }
    }
}
