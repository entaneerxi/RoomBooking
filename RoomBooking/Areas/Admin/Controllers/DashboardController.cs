using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Data;

namespace RoomBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var today = DateTime.Today;
            var thisMonth = new DateTime(today.Year, today.Month, 1);

            ViewBag.TotalRooms = await _context.Rooms.CountAsync(r => r.IsActive);
            ViewBag.AvailableRooms = await _context.Rooms.CountAsync(r => r.IsActive && r.Status == Models.RoomStatus.Available);
            ViewBag.TotalBookings = await _context.Bookings.CountAsync();
            ViewBag.PendingBookings = await _context.Bookings.CountAsync(b => b.Status == Models.BookingStatus.Pending);
            ViewBag.TodayCheckIns = await _context.Bookings.CountAsync(b => b.CheckInDate.Date == today && b.Status == Models.BookingStatus.Confirmed);
            ViewBag.TodayCheckOuts = await _context.Bookings.CountAsync(b => b.CheckOutDate.Date == today && b.Status == Models.BookingStatus.CheckedIn);
            ViewBag.MonthlyRevenue = await _context.Payments
                .Where(p => p.Status == Models.PaymentStatus.Paid && p.PaidAt >= thisMonth)
                .SumAsync(p => p.Amount);

            var recentBookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .OrderByDescending(b => b.CreatedAt)
                .Take(10)
                .ToListAsync();

            return View(recentBookings);
        }
    }
}
