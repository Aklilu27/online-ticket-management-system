using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineTicketManagementSystem.Models;

namespace OnlineTicketManagementSystem.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Companies.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var company = await _context.Companies
                .Include(c => c.Users)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (company == null) return NotFound();

            return View(company);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Location")] Company company)
        {
            if (!ModelState.IsValid) return View(company);

            company.CreatedAt = DateTime.Now;
            _context.Add(company);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var company = await _context.Companies.FindAsync(id);
            if (company == null) return NotFound();

            return View(company);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Location,CreatedAt")] Company company)
        {
            if (id != company.Id) return NotFound();
            if (!ModelState.IsValid) return View(company);

            try
            {
                _context.Update(company);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Companies.AnyAsync(e => e.Id == company.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var company = await _context.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null) return NotFound();

            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company != null)
            {
                _context.Companies.Remove(company);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
