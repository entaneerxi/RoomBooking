using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Data;
using RoomBooking.Models;

namespace RoomBooking.Controllers
{
    public class RoomsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoomsController> _logger;

        public RoomsController(ApplicationDbContext context, ILogger<RoomsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Rooms
        public async Task<IActionResult> Index(string? roomType, decimal? minPrice, decimal? maxPrice)
        {
            var roomsQuery = _context.Rooms.Where(r => r.IsActive);

            if (!string.IsNullOrEmpty(roomType))
            {
                roomsQuery = roomsQuery.Where(r => r.RoomType == roomType);
            }

            if (minPrice.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.DailyRate >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                roomsQuery = roomsQuery.Where(r => r.DailyRate <= maxPrice.Value);
            }

            var rooms = await roomsQuery.OrderBy(r => r.RoomNumber).ToListAsync();

            ViewBag.RoomTypes = await _context.Rooms
                .Where(r => r.IsActive)
                .Select(r => r.RoomType)
                .Distinct()
                .ToListAsync();

            return View(rooms);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _context.Rooms
                .FirstOrDefaultAsync(m => m.Id == id);

            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/CheckAvailability
        public async Task<IActionResult> CheckAvailability(int roomId, DateTime checkIn, DateTime checkOut)
        {
            var conflictingBookings = await _context.Bookings
                .Where(b => b.RoomId == roomId &&
                           b.Status != BookingStatus.Cancelled &&
                           b.CheckInDate < checkOut &&
                           b.CheckOutDate > checkIn)
                .ToListAsync();

            var isAvailable = !conflictingBookings.Any();
            return Json(new { available = isAvailable });
        }
    }
}
