using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Data;
using RoomBooking.Models;

namespace RoomBooking.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Slides = await _context.HomepageSlides
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();

            ViewBag.FeaturedRooms = await _context.Rooms
                .Where(r => r.IsActive && r.Status == RoomStatus.Available)
                .Take(6)
                .ToListAsync();

            ViewBag.Promotions = await _context.Promotions
                .Where(p => p.IsActive && p.StartDate <= DateTime.UtcNow && p.EndDate >= DateTime.UtcNow)
                .OrderBy(p => p.DisplayOrder)
                .Take(3)
                .ToListAsync();

            ViewBag.Facilities = await _context.Facilities
                .Where(f => f.IsActive)
                .OrderBy(f => f.DisplayOrder)
                .Take(6)
                .ToListAsync();

            ViewBag.ContactInfo = await _context.ContactInfos.FirstOrDefaultAsync();
            ViewBag.SocialMedia = await _context.SocialMedias
                .Where(s => s.IsActive)
                .OrderBy(s => s.DisplayOrder)
                .ToListAsync();

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
