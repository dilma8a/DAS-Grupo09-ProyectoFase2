using Microsoft.AspNetCore.Mvc;
using DAS_Grupo09_ProyectoFase2.Models;
using DAS_Grupo09_ProyectoFase2.Services;

namespace DAS_Grupo09_ProyectoFase2.Controllers
{
    public class ReportesController : Controller
    {
        private readonly IReporteService _reporteService;

        public ReportesController(IReporteService reporteService)
        {
            _reporteService = reporteService;
        }

        private bool UsuarioNoAutenticado()
        {
            return string.IsNullOrEmpty(HttpContext.Session.GetString("Token"));
        }

        // GET: Reportes
        public async Task<IActionResult> Index()
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            try
            {
                var estadisticas = await _reporteService.GetEstadisticasGeneralesAsync();
                return View(estadisticas);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cargar estadísticas: {ex.Message}";
                return View(new EstadisticasGenerales());
            }
        }

        // GET: Reportes/Envios
        public async Task<IActionResult> Envios(DateTime? fechaInicio, DateTime? fechaFin, string? estado)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            try
            {
                ViewBag.FechaInicio = fechaInicio;
                ViewBag.FechaFin = fechaFin;
                ViewBag.Estado = estado;

                var reportes = await _reporteService.GetReporteEnviosAsync(fechaInicio, fechaFin, estado);
                var estadisticas = await _reporteService.GetEstadisticasEnviosPorEstadoAsync();

                ViewBag.Estadisticas = estadisticas;

                return View(reportes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al generar reporte: {ex.Message}";
                return View(new List<ReporteEnvio>());
            }
        }

        // GET: Reportes/Reclamos
        public async Task<IActionResult> Reclamos(DateTime? fechaInicio, DateTime? fechaFin, string? estado)
        {
            if (UsuarioNoAutenticado())
                return RedirectToAction("Index", "Login");

            try
            {
                ViewBag.FechaInicio = fechaInicio;
                ViewBag.FechaFin = fechaFin;
                ViewBag.Estado = estado;

                var reportes = await _reporteService.GetReporteReclamosAsync(fechaInicio, fechaFin, estado);
                var estadisticas = await _reporteService.GetEstadisticasReclamosPorEstadoAsync();

                ViewBag.Estadisticas = estadisticas;

                return View(reportes);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al generar reporte: {ex.Message}";
                return View(new List<ReporteReclamo>());
            }
        }
    }
}
