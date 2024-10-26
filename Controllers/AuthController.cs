using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jiji.Models;
using System.Security.Cryptography;
using System.Text;

namespace Jiji.Controllers
{
    public class AuthController : Controller
    {
        private readonly TestContext _context;

        public AuthController(TestContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var hashedPassword = HashPassword(password);

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == hashedPassword);

            if (usuario != null)
            {
                HttpContext.Session.SetInt32("UserId", usuario.Id);
                HttpContext.Session.SetString("UserName", usuario.Nombre);
                HttpContext.Session.SetInt32("UserRole", usuario.Idrol);
                HttpContext.Session.SetString("UserEmail", usuario.Email);
                if (usuario.Idrol == 1)
                {
                    return RedirectToAction("Index", "Administrador");
                }
                else if (usuario.Idrol == 2)
                {
                    return RedirectToAction("Index", "Cliente");
                }
            }

            ViewData["ErrorMessage"] = "Usuario o contraseña incorrectos.";
            return View("Index");
        }

        private static string HashPassword(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Auth");
        }

    }
}
