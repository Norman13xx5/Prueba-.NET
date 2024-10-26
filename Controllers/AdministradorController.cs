using Microsoft.AspNetCore.Mvc;
using Jiji.Models;

namespace Jiji.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly TestContext _context;

        public AdministradorController(TestContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetInt32("UserRole");

            if (userRole == 1)
            {
                return View();
            }
            else if (userRole == 2)
            {
                return RedirectToAction("Index", "Cliente");
            }

            ViewData["ErrorMessage"] = "Acceso denegado. Solo los administradores pueden acceder a esta vista.";
            return View("Error");
        }

    }
}