using Microsoft.AspNetCore.Mvc;

namespace DAS_Grupo09_ProyectoFase2.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // Verificar si el usuario está autenticado
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Login");
            }

            // Pasar información del usuario a la vista
            ViewBag.Usuario = HttpContext.Session.GetString("Usuario");
            ViewBag.NombreCompleto = HttpContext.Session.GetString("NombreCompleto");
            ViewBag.Rol = HttpContext.Session.GetString("Rol");

            return View();
        }
    }
}
