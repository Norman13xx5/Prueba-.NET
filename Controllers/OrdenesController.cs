using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jiji.Models;

namespace Jiji.Controllers
{
    public class OrdenesController : Controller
    {
        private readonly TestContext _context;

        public OrdenesController(TestContext context)
        {
            _context = context;
        }

        private IActionResult ValidacionRol()
        {
            var userRole = HttpContext.Session.GetInt32("UserRole");

            if (userRole == 1)
            {
                return null;
            }
            else if (userRole == 2)
            {
                return RedirectToAction("Index", "Cliente");
            }

            ViewData["ErrorMessage"] = "Acceso denegado. Solo los administradores pueden acceder a esta vista.";
            return View("Error");
        }

        public async Task<IActionResult> Index()
        {
            var result = ValidacionRol();
            if (result != null) return result;

            var ordenes = _context.Ordenes
                        .Include(p => p.IdusuarioNavigation)
                        .ToListAsync();
            return ordenes != null ?
                        View(await ordenes) :
                        Problem("Entity set 'JijiContext.ordenes' is null.");
        }

        [HttpGet]
        public IActionResult Create()
        {
            var result = ValidacionRol();
            if (result != null) return result;

            ViewBag.Usuarios = _context.Usuarios.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ordene ordene)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            _context.Add(ordene);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            if (id == null)
            {
                return NotFound();
            }

            var orden = await _context.Ordenes.FindAsync(id);
            if (orden == null)
            {
                return NotFound();
            }

            ViewBag.Usuarios = await _context.Usuarios.ToListAsync();

            return View(orden);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSave(int id, [Bind("Id,Nombre,IdUsuario,CreatedAt")] Ordene ordene)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            var existingOrdene = await _context.Ordenes.FindAsync(id);
            if (existingOrdene == null)
            {
                return NotFound();
            }

            existingOrdene.Nombre = ordene.Nombre;
            existingOrdene.CreatedAt = ordene.CreatedAt;
            existingOrdene.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            if (id == null || _context.Ordenes == null)
            {
                return NotFound();
            }

            var ordene = await _context.Ordenes
                .Include(o => o.IdusuarioNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (ordene == null)
            {
                return NotFound();
            }

            return View(ordene);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = ValidacionRol();
            if (result != null) return result;
            
            var ordene = await _context.Ordenes.FindAsync(id);
            if (ordene == null)
            {
                return NotFound();
            }

            _context.Ordenes.Remove(ordene);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
