using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Data;

namespace RoomBooking.Controllers
{
    public class GalleryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GalleryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Gallery
        public async Task<IActionResult> Index(string? category)
        {
            var galleryQuery = _context.Galleries.Where(g => g.IsActive);

            if (!string.IsNullOrEmpty(category))
            {
                galleryQuery = galleryQuery.Where(g => g.Category == category);
            }

            var galleries = await galleryQuery
                .OrderBy(g => g.DisplayOrder)
                .ToListAsync();

            ViewBag.Categories = await _context.Galleries
                .Where(g => g.IsActive && !string.IsNullOrEmpty(g.Category))
                .Select(g => g.Category)
                .Distinct()
                .ToListAsync();

            return View(galleries);
        }
    }
}
