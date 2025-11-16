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
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<PaymentsController> _logger;

        public PaymentsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment environment,
            ILogger<PaymentsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _environment = environment;
            _logger = logger;
        }

        // GET: Payments/Create
        public async Task<IActionResult> Create(int bookingId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Challenge();
            }

            var booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == bookingId && b.UserId == user.Id);

            if (booking == null)
            {
                return NotFound();
            }

            var paymentMethods = await _context.PaymentMethods
                .Where(pm => pm.IsActive)
                .OrderBy(pm => pm.DisplayOrder)
                .ToListAsync();

            var model = new PaymentViewModel
            {
                BookingId = bookingId,
                Booking = booking,
                PaymentMethods = paymentMethods
            };

            return View(model);
        }

        // POST: Payments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PaymentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Challenge();
                }

                var booking = await _context.Bookings
                    .FirstOrDefaultAsync(b => b.Id == model.BookingId && b.UserId == user.Id);

                if (booking == null)
                {
                    return NotFound();
                }

                string? paymentProofUrl = null;
                if (model.PaymentProof != null)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "payments");
                    Directory.CreateDirectory(uploadsFolder);
                    
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.PaymentProof.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.PaymentProof.CopyToAsync(fileStream);
                    }
                    
                    paymentProofUrl = "/uploads/payments/" + uniqueFileName;
                }

                var payment = new Payment
                {
                    BookingId = model.BookingId,
                    UserId = user.Id,
                    Amount = booking.FinalAmount,
                    PaymentMethodId = model.PaymentMethodId,
                    Status = PaymentStatus.Pending,
                    TransactionReference = model.TransactionReference,
                    PaymentProofUrl = paymentProofUrl,
                    Notes = model.Notes,
                    CreatedAt = DateTime.UtcNow
                };

                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Bookings", new { id = model.BookingId });
            }

            model.Booking = await _context.Bookings
                .Include(b => b.Room)
                .FirstOrDefaultAsync(b => b.Id == model.BookingId);

            model.PaymentMethods = await _context.PaymentMethods
                .Where(pm => pm.IsActive)
                .OrderBy(pm => pm.DisplayOrder)
                .ToListAsync();

            return View(model);
        }
    }
}
