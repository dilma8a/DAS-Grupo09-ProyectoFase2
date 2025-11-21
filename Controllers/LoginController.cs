using DAS_Grupo09_ProyectoFase2.Models;
using DAS_Grupo09_ProyectoFase2.Services;
using Microsoft.AspNetCore.Mvc;

namespace DAS_Grupo09_ProyectoFase2.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Si ya está autenticado, redirigir al dashboard
            if (HttpContext.Session.GetString("Token") != null)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _loginService.LoginAsync(model.Usuario, model.Clave);

            if (response.code == 0 && response.token != null && response.usuario != null)
            {
                // Login exitoso - Guardar información en sesión
                HttpContext.Session.SetString("Token", response.token);
                HttpContext.Session.SetString("Usuario", response.usuario.username);
                HttpContext.Session.SetString("NombreCompleto", $"{response.usuario.nombre} {response.usuario.apellido}");
                HttpContext.Session.SetString("Rol", response.usuario.rol);
                HttpContext.Session.SetInt32("IdRol", response.usuario.idRol);
                HttpContext.Session.SetInt32("IdUsuario", response.usuario.id);

                return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                // Login fallido
                ViewBag.Error = response.msj;
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
