using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Data;
using RoomBooking.Models;

namespace RoomBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class MonthlyRentalsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MonthlyRentalsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/MonthlyRentals
        public async Task<IActionResult> Index()
        {
            var rentals = await _context.MonthlyRentals
                .Include(m => m.Booking)
                    .ThenInclude(b => b.Room)
                .Include(m => m.Booking)
                    .ThenInclude(b => b.User)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            return View(rentals);
        }

        // GET: Admin/MonthlyRentals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var rental = await _context.MonthlyRentals
                .Include(m => m.Booking)
                    .ThenInclude(b => b.Room)
                .Include(m => m.Booking)
                    .ThenInclude(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (rental == null) return NotFound();
            return View(rental);
        }

        // GET: Admin/MonthlyRentals/Create
        public async Task<IActionResult> Create(int? bookingId)
        {
            ViewBag.Bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.BookingType == BookingType.Monthly && b.Status == BookingStatus.CheckedIn)
                .ToListAsync();

            var rental = new MonthlyRental();
            if (bookingId.HasValue)
            {
                rental.BookingId = bookingId.Value;
            }

            return View(rental);
        }

        // POST: Admin/MonthlyRentals/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,WaterBill,ElectricityBill,PreviousWaterReading,CurrentWaterReading,PreviousElectricityReading,CurrentElectricityReading,WaterUnitPrice,ElectricityUnitPrice,BillingPeriodStart,BillingPeriodEnd,PaymentStatus,Notes")] MonthlyRental rental)
        {
            if (ModelState.IsValid)
            {
                // Calculate bills if not provided
                if (rental.WaterBill == 0)
                {
                    var waterUsage = rental.CurrentWaterReading - rental.PreviousWaterReading;
                    rental.WaterBill = waterUsage * rental.WaterUnitPrice;
                }

                if (rental.ElectricityBill == 0)
                {
                    var elecUsage = rental.CurrentElectricityReading - rental.PreviousElectricityReading;
                    rental.ElectricityBill = elecUsage * rental.ElectricityUnitPrice;
                }

                rental.CreatedAt = DateTime.UtcNow;
                _context.Add(rental);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.BookingType == BookingType.Monthly && b.Status == BookingStatus.CheckedIn)
                .ToListAsync();

            return View(rental);
        }

        // GET: Admin/MonthlyRentals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var rental = await _context.MonthlyRentals.FindAsync(id);
            if (rental == null) return NotFound();

            ViewBag.Bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.BookingType == BookingType.Monthly)
                .ToListAsync();

            return View(rental);
        }

        // POST: Admin/MonthlyRentals/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookingId,WaterBill,ElectricityBill,PreviousWaterReading,CurrentWaterReading,PreviousElectricityReading,CurrentElectricityReading,WaterUnitPrice,ElectricityUnitPrice,BillingPeriodStart,BillingPeriodEnd,PaymentStatus,PaidDate,Notes,CreatedAt")] MonthlyRental rental)
        {
            if (id != rental.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    // Recalculate bills
                    var waterUsage = rental.CurrentWaterReading - rental.PreviousWaterReading;
                    rental.WaterBill = waterUsage * rental.WaterUnitPrice;

                    var elecUsage = rental.CurrentElectricityReading - rental.PreviousElectricityReading;
                    rental.ElectricityBill = elecUsage * rental.ElectricityUnitPrice;

                    rental.UpdatedAt = DateTime.UtcNow;
                    _context.Update(rental);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MonthlyRentalExists(rental.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.BookingType == BookingType.Monthly)
                .ToListAsync();

            return View(rental);
        }

        // POST: Admin/MonthlyRentals/ConfirmPayment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var rental = await _context.MonthlyRentals.FindAsync(id);
            if (rental == null) return NotFound();

            rental.PaymentStatus = PaymentStatus.Paid;
            rental.PaidDate = DateTime.UtcNow;
            rental.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

        private bool MonthlyRentalExists(int id)
        {
            return _context.MonthlyRentals.Any(e => e.Id == id);
        }
    }
}
