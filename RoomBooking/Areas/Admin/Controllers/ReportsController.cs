using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoomBooking.Services;

namespace RoomBooking.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff")]
    public class ReportsController : Controller
    {
        private readonly ReportService _reportService;

        public ReportsController(ReportService reportService)
        {
            _reportService = reportService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookingReport(DateTime startDate, DateTime endDate)
        {
            var pdfBytes = await _reportService.GenerateBookingReport(startDate, endDate);
            return File(pdfBytes, "application/pdf", $"BookingReport_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
        }

        [HttpPost]
        public async Task<IActionResult> MonthlyReport(int year, int month)
        {
            var pdfBytes = await _reportService.GenerateMonthlyReport(year, month);
            return File(pdfBytes, "application/pdf", $"MonthlyReport_{year}_{month:D2}.pdf");
        }

        [HttpPost]
        public async Task<IActionResult> UtilitiesReport(DateTime startDate, DateTime endDate)
        {
            var pdfBytes = await _reportService.GenerateUtilitiesReport(startDate, endDate);
            return File(pdfBytes, "application/pdf", $"UtilitiesReport_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.pdf");
        }
    }
}
