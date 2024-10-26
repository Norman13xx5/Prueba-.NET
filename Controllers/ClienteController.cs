using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Jiji.Models;

namespace Jiji.Controllers
{
    public class ClienteController : Controller
    {
        private readonly TestContext _context;

        public ClienteController(TestContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userRole = HttpContext.Session.GetInt32("UserRole");

            if (userRole == 2)
            {
                var ordenes = _context.Ordenes
                    .Where(o => o.Idusuario == userId)
                    .ToList();

                return View(ordenes);
            }
            else if (userRole == 1)
            {
                return RedirectToAction("Index", "Administrador");
            }

            ViewData["ErrorMessage"] = "Acceso denegado. Solo los administradores pueden acceder a esta vista.";
            return View("Error");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var productos = _context.Productos
                .Where(p => p.Idorden == id)
                .Include(p => p.IdordenNavigation)
                .Include(p => p.IdcategoriaNavigation)
                .Include(p => p.IdempresaNavigation)
                .ToList();

            if (!productos.Any())
            {
                return Redirect("https://www.google.com");
            }

            return View(productos);

        }

    }
}