using Microsoft.AspNetCore.Mvc;
using DAS_Grupo09_ProyectoFase2.Models;
using DAS_Grupo09_ProyectoFase2.Services;

namespace DAS_Grupo09_ProyectoFase2.Controllers
{
    public class TrackingController : Controller
    {
        private readonly IPaqueteService _paqueteService;

        public TrackingController(IPaqueteService paqueteService)
        {
            _paqueteService = paqueteService;
        }

        // GET: Tracking
        public IActionResult Index()
        {
            // Tracking público; si quieres que requiera login,
            // añade aquí el mismo chequeo de sesión que en los demás controladores.
            return View();
        }

        // POST: Tracking/Buscar
        [HttpPost]
        public async Task<IActionResult> Buscar(string codigoBarra)
        {
            if (string.IsNullOrWhiteSpace(codigoBarra))
            {
                ViewBag.Error = "Debe ingresar un código de seguimiento";
                return View("Index");
            }

            var paquete = await _paqueteService.ObtenerPorCodigoAsync(codigoBarra);

            if (paquete == null)
            {
                ViewBag.Error = "No se encontró ningún paquete con ese código";
                return View("Index");
            }

            return View("Resultado", paquete);
        }
    }
}
