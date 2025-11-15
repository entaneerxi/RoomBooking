using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RoomBooking.Data;
using RoomBooking.Models;

namespace RoomBooking.Services
{
    public class ReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
            QuestPDF.Settings.License = LicenseType.Community;
        }

        public async Task<byte[]> GenerateBookingReport(DateTime startDate, DateTime endDate)
        {
            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.CreatedAt >= startDate && b.CreatedAt <= endDate)
                .OrderByDescending(b => b.CreatedAt)
                .ToListAsync();

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text("Booking Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(5);

                            column.Item().Text($"Report Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
                            column.Item().Text($"Total Bookings: {bookings.Count}");

                            column.Item().PaddingTop(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1.5f);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("ID");
                                    header.Cell().Element(CellStyle).Text("Room");
                                    header.Cell().Element(CellStyle).Text("Guest");
                                    header.Cell().Element(CellStyle).Text("Check-In");
                                    header.Cell().Element(CellStyle).Text("Status");
                                    header.Cell().Element(CellStyle).Text("Amount");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.DefaultTextStyle(x => x.SemiBold())
                                            .PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                    }
                                });

                                foreach (var booking in bookings)
                                {
                                    table.Cell().Element(CellStyle).Text(booking.Id.ToString());
                                    table.Cell().Element(CellStyle).Text(booking.Room?.Name ?? "N/A");
                                    table.Cell().Element(CellStyle).Text($"{booking.User?.FirstName} {booking.User?.LastName}");
                                    table.Cell().Element(CellStyle).Text(booking.CheckInDate.ToString("yyyy-MM-dd"));
                                    table.Cell().Element(CellStyle).Text(booking.Status.ToString());
                                    table.Cell().Element(CellStyle).Text($"${booking.FinalAmount:N2}");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                            .PaddingVertical(5);
                                    }
                                }
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                });
            }).GeneratePdf();
        }

        public async Task<byte[]> GenerateMonthlyReport(int year, int month)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            var bookings = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .Where(b => b.CreatedAt >= startDate && b.CreatedAt <= endDate)
                .ToListAsync();

            var payments = await _context.Payments
                .Where(p => p.PaidAt >= startDate && p.PaidAt <= endDate && p.Status == PaymentStatus.Paid)
                .ToListAsync();

            var totalRevenue = payments.Sum(p => p.Amount);

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text($"Monthly Report - {startDate:MMMM yyyy}")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(10);

                            column.Item().Text($"Report Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
                            column.Item().Text($"Total Bookings: {bookings.Count}");
                            column.Item().Text($"Total Revenue: ${totalRevenue:N2}");
                            column.Item().Text($"Average Booking Value: ${(bookings.Count > 0 ? totalRevenue / bookings.Count : 0):N2}");

                            column.Item().PaddingTop(10).Text("Booking Status Summary").SemiBold().FontSize(14);
                            
                            foreach (var status in Enum.GetValues<BookingStatus>())
                            {
                                var count = bookings.Count(b => b.Status == status);
                                if (count > 0)
                                {
                                    column.Item().Text($"{status}: {count}");
                                }
                            }
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                        });
                });
            }).GeneratePdf();
        }

        public async Task<byte[]> GenerateUtilitiesReport(DateTime startDate, DateTime endDate)
        {
            var rentals = await _context.MonthlyRentals
                .Include(m => m.Booking)
                    .ThenInclude(b => b.Room)
                .Include(m => m.Booking)
                    .ThenInclude(b => b.User)
                .Where(m => m.BillingPeriodStart >= startDate && m.BillingPeriodEnd <= endDate)
                .ToListAsync();

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header()
                        .Text("Water & Electricity Report")
                        .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                    page.Content()
                        .PaddingVertical(1, Unit.Centimetre)
                        .Column(column =>
                        {
                            column.Spacing(5);

                            column.Item().Text($"Report Period: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}");
                            column.Item().Text($"Total Rentals: {rentals.Count}");

                            column.Item().PaddingTop(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(2);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1.5f);
                                    columns.RelativeColumn(1.5f);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Room");
                                    header.Cell().Element(CellStyle).Text("Tenant");
                                    header.Cell().Element(CellStyle).Text("Water Usage");
                                    header.Cell().Element(CellStyle).Text("Electricity Usage");
                                    header.Cell().Element(CellStyle).Text("Water Bill");
                                    header.Cell().Element(CellStyle).Text("Electricity Bill");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.DefaultTextStyle(x => x.SemiBold())
                                            .PaddingVertical(5).BorderBottom(1).BorderColor(Colors.Black);
                                    }
                                });

                                foreach (var rental in rentals)
                                {
                                    var waterUsage = rental.CurrentWaterReading - rental.PreviousWaterReading;
                                    var elecUsage = rental.CurrentElectricityReading - rental.PreviousElectricityReading;

                                    table.Cell().Element(CellStyle).Text(rental.Booking?.Room?.Name ?? "N/A");
                                    table.Cell().Element(CellStyle).Text($"{rental.Booking?.User?.FirstName} {rental.Booking?.User?.LastName}");
                                    table.Cell().Element(CellStyle).Text($"{waterUsage:N2}");
                                    table.Cell().Element(CellStyle).Text($"{elecUsage:N2}");
                                    table.Cell().Element(CellStyle).Text($"${rental.WaterBill:N2}");
                                    table.Cell().Element(CellStyle).Text($"${rental.ElectricityBill:N2}");

                                    static IContainer CellStyle(IContainer container)
                                    {
                                        return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2)
                                            .PaddingVertical(5);
                                    }
                                }
                            });
                        });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Page ");
                            x.CurrentPageNumber();
                            x.Span(" of ");
                            x.TotalPages();
                        });
                });
            }).GeneratePdf();
        }
    }
}
