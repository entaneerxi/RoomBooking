using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Data;
using RoomBooking.Models;

namespace RoomBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class HomepageSlidesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomepageSlidesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.HomepageSlides.OrderBy(h => h.DisplayOrder).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var slide = await _context.HomepageSlides.FirstOrDefaultAsync(m => m.Id == id);
            if (slide == null) return NotFound();
            return View(slide);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Subtitle,ImageUrl,LinkUrl,ButtonText,IsActive,DisplayOrder")] HomepageSlide slide)
        {
            if (ModelState.IsValid)
            {
                slide.CreatedAt = DateTime.UtcNow;
                _context.Add(slide);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(slide);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var slide = await _context.HomepageSlides.FindAsync(id);
            if (slide == null) return NotFound();
            return View(slide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Subtitle,ImageUrl,LinkUrl,ButtonText,IsActive,DisplayOrder,CreatedAt")] HomepageSlide slide)
        {
            if (id != slide.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    slide.UpdatedAt = DateTime.UtcNow;
                    _context.Update(slide);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlideExists(slide.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(slide);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var slide = await _context.HomepageSlides.FirstOrDefaultAsync(m => m.Id == id);
            if (slide == null) return NotFound();
            return View(slide);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var slide = await _context.HomepageSlides.FindAsync(id);
            if (slide != null)
            {
                _context.HomepageSlides.Remove(slide);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool SlideExists(int id)
        {
            return _context.HomepageSlides.Any(e => e.Id == id);
        }
    }
}
