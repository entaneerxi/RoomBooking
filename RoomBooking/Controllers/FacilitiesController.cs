using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Data;

namespace RoomBooking.Controllers
{
    public class FacilitiesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FacilitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Facilities
        public async Task<IActionResult> Index()
        {
            var facilities = await _context.Facilities
                .Where(f => f.IsActive)
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();

            return View(facilities);
        }
    }
}
