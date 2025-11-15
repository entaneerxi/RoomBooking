using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Data;
using RoomBooking.Models;
using RoomBooking.ViewModels;

namespace RoomBooking.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<BookingsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Bookings/Create
        public async Task<IActionResult> Create(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return NotFound();
            }

            var model = new BookingViewModel
            {
                RoomId = roomId,
                Room = room,
                CheckInDate = DateTime.Today.AddDays(1),
                CheckOutDate = DateTime.Today.AddDays(2)
            };

            return View(model);
        }

        // POST: Bookings/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Challenge();
                }

                var room = await _context.Rooms.FindAsync(model.RoomId);
                if (room == null)
                {
                    return NotFound();
                }

                // Check availability
                var conflictingBookings = await _context.Bookings
                    .Where(b => b.RoomId == model.RoomId &&
                               b.Status != BookingStatus.Cancelled &&
                               b.CheckInDate < model.CheckOutDate &&
                               b.CheckOutDate > model.CheckInDate)
                    .ToListAsync();

                if (conflictingBookings.Any())
                {
                    ModelState.AddModelError("", "This room is not available for the selected dates.");
                    model.Room = room;
                    return View(model);
                }

                // Calculate total amount
                var days = (model.CheckOutDate - model.CheckInDate).Days;
                var totalAmount = model.BookingType == BookingType.Daily
                    ? room.DailyRate * days
                    : room.MonthlyRate;

                var booking = new Booking
                {
                    UserId = user.Id,
                    RoomId = model.RoomId,
                    BookingType = model.BookingType,
                    CheckInDate = model.CheckInDate,
                    CheckOutDate = model.CheckOutDate,
                    NumberOfGuests = model.NumberOfGuests,
                    TotalAmount = totalAmount,
                    DiscountAmount = 0,
                    FinalAmount = totalAmount,
                    SpecialRequests = model.SpecialRequests,
                    Status = BookingStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyBookings));
            }

            model.Room = await _context.Rooms.FindAsync(model.RoomId);
            return View(model);
        }

        // GET: Bookings/MyBookings
        public async Task<IActionResult> MyBookings()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Payments)
                .Where(b => b.UserId == user.Id)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return View(bookings);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.Payments)
                    .ThenInclude(p => p.PaymentMethod)
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }

        // GET: Bookings/RequestPostpone/5
        public async Task<IActionResult> RequestPostpone(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);

            if (booking == null)
            {
                return NotFound();
            }

            if (booking.Status != BookingStatus.Confirmed && booking.Status != BookingStatus.Pending)
            {
                return BadRequest("Cannot postpone this booking.");
            }

            return View(booking);
        }

        // POST: Bookings/RequestPostpone/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestPostpone(int id, DateTime newCheckInDate, DateTime newCheckOutDate, string postponeReason)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);

            if (booking == null)
            {
                return NotFound();
            }

            booking.Status = BookingStatus.PostponeRequested;
            booking.PostponeRequestedDate = DateTime.UtcNow;
            booking.NewCheckInDate = newCheckInDate;
            booking.NewCheckOutDate = newCheckOutDate;
            booking.PostponeReason = postponeReason;
            booking.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MyBookings));
        }
    }
}
