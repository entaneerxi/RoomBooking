using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Data;
using RoomBooking.Models;

namespace RoomBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class BookingAddonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookingAddonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.BookingAddons.OrderBy(b => b.DisplayOrder).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var addon = await _context.BookingAddons.FirstOrDefaultAsync(m => m.Id == id);
            if (addon == null) return NotFound();
            return View(addon);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,IconClass,IsActive,DisplayOrder")] BookingAddon addon)
        {
            if (ModelState.IsValid)
            {
                addon.CreatedAt = DateTime.UtcNow;
                _context.Add(addon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(addon);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var addon = await _context.BookingAddons.FindAsync(id);
            if (addon == null) return NotFound();
            return View(addon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,IconClass,IsActive,DisplayOrder,CreatedAt")] BookingAddon addon)
        {
            if (id != addon.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    addon.UpdatedAt = DateTime.UtcNow;
                    _context.Update(addon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddonExists(addon.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(addon);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var addon = await _context.BookingAddons.FirstOrDefaultAsync(m => m.Id == id);
            if (addon == null) return NotFound();
            return View(addon);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var addon = await _context.BookingAddons.FindAsync(id);
            if (addon != null)
            {
                _context.BookingAddons.Remove(addon);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool AddonExists(int id)
        {
            return _context.BookingAddons.Any(e => e.Id == id);
        }
    }
}
