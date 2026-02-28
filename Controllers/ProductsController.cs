using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineTicketManagementSystem.Models;

namespace OnlineTicketManagementSystem.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Company)
                .AsNoTracking()
                .ToListAsync();

            return View(products);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null) return NotFound();

            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            await LoadCompaniesAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductName,CompanyId")] Product product)
        {
            if (!ModelState.IsValid)
            {
                await LoadCompaniesAsync(product.CompanyId);
                return View(product);
            }

            product.CreatedAt = DateTime.Now;
            _context.Add(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            await LoadCompaniesAsync(product.CompanyId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,CompanyId,CreatedAt")] Product product)
        {
            if (id != product.ProductId) return NotFound();

            if (!ModelState.IsValid)
            {
                await LoadCompaniesAsync(product.CompanyId);
                return View(product);
            }

            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Products.AnyAsync(e => e.ProductId == product.ProductId))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.Company)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null) return NotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadCompaniesAsync(object selectedCompany = null)
        {
            var companies = await _context.Companies.OrderBy(c => c.Name).ToListAsync();
            ViewBag.Companies = new SelectList(companies, "Id", "Name", selectedCompany);
        }
    }
}
