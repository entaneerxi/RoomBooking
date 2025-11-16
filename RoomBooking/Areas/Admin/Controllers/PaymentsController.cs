using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoomBooking.Data;
using RoomBooking.Models;

namespace RoomBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class PaymentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin/Payments
        public async Task<IActionResult> Index(PaymentStatus? status)
        {
            var paymentsQuery = _context.Payments
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Room)
                .Include(p => p.User)
                .Include(p => p.PaymentMethod)
                .AsQueryable();

            if (status.HasValue)
            {
                paymentsQuery = paymentsQuery.Where(p => p.Status == status.Value);
            }

            var payments = await paymentsQuery
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(payments);
        }

        // GET: Admin/Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var payment = await _context.Payments
                .Include(p => p.Booking)
                    .ThenInclude(b => b.Room)
                .Include(p => p.User)
                .Include(p => p.PaymentMethod)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // POST: Admin/Payments/ConfirmPayment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized();
            }

            payment.Status = PaymentStatus.Paid;
            payment.PaidAt = DateTime.UtcNow;
            payment.ConfirmedBy = currentUser.Id;
            payment.ConfirmedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Admin/Payments/RejectPayment/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectPayment(int id, string? notes)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            payment.Status = PaymentStatus.Failed;
            payment.Notes = notes;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
