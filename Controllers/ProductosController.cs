using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Jiji.Models;

namespace Jiji.Controllers
{
    public class ProductosController : Controller
    {
        private readonly TestContext _context;

        public ProductosController(TestContext context)
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

            var productos = _context.Productos
                        .Include(p => p.IdordenNavigation)
                        .Include(p => p.IdcategoriaNavigation)
                        .Include(p => p.IdempresaNavigation)
                        .ToListAsync();
            return productos != null ?
                        View(await productos) :
                        Problem("Entity set 'JijiContext.Productos' is null.");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewBag.Ordenes = await _context.Ordenes.ToListAsync();
            ViewBag.Categorias = await _context.Categorias.ToListAsync();
            ViewBag.Empresas = await _context.Empresas.ToListAsync();

            return View(producto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSave(int id, [Bind("Id,Nombre,Idorden,Idcategoria,Idempresa,Valor")] Producto producto)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            var existingProducto = await _context.Productos.FindAsync(id);
            if (existingProducto == null)
            {
                return NotFound();
            }

            existingProducto.Nombre = producto.Nombre;
            existingProducto.Idorden = producto.Idorden;
            existingProducto.Idcategoria = producto.Idcategoria;
            existingProducto.Idempresa = producto.Idempresa;
            existingProducto.Valor = producto.Valor;
            existingProducto.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Details(int? id)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdordenNavigation)
                .Include(p => p.IdcategoriaNavigation)
                .Include(p => p.IdempresaNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }


        [HttpGet]
        public IActionResult Create()
        {
            var result = ValidacionRol();
            if (result != null) return result;

            ViewBag.Ordenes = _context.Ordenes.ToList();
            ViewBag.Categorias = _context.Categorias.ToList();
            ViewBag.Empresas = _context.Empresas.ToList();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            var result = ValidacionRol();
            if (result != null) return result;

            _context.Add(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var result = ValidacionRol();
            if (result != null) return result;
            
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
