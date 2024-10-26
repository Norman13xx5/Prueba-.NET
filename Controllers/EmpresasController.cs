using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jiji.Models;

namespace Jiji.Controllers
{
    public class EmpresasController : Controller
    {
        private readonly TestContext _context;

        public EmpresasController(TestContext context)
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

            return _context.Empresas != null ?
                        View(await _context.Empresas.ToListAsync()) :
                        Problem("Entity set 'JijiContext.Usuarios'  is null.");
        }

        public async Task<IActionResult> Details(int? id)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empresa == null)
            {
                return NotFound();
            }

            return View(empresa);
        }

        public IActionResult Create()
        {
            var result = ValidacionRol();
            if (result != null) return result;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Empresa empresa)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            if (ModelState.IsValid)
            {
                _context.Empresas.Add(empresa);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(empresa);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            if (id == null || _context.Empresas == null)
            {
                return NotFound();
            }

            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return View(empresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSave(int id, [Bind("Id,Nombre,Direccion,Telefono")] Empresa empresa)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            if (id != empresa.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(empresa);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(empresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = ValidacionRol();
            if (result != null) return result;
            
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return NotFound();
            }

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
