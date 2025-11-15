using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomBooking.Models
{
    public class MonthlyRental
    {
        public int Id { get; set; }
        
        [Required]
        public int BookingId { get; set; }
        
        public decimal WaterBill { get; set; }
        
        public decimal ElectricityBill { get; set; }
        
        public decimal PreviousWaterReading { get; set; }
        
        public decimal CurrentWaterReading { get; set; }
        
        public decimal PreviousElectricityReading { get; set; }
        
        public decimal CurrentElectricityReading { get; set; }
        
        public decimal WaterUnitPrice { get; set; }
        
        public decimal ElectricityUnitPrice { get; set; }
        
        public DateTime BillingPeriodStart { get; set; }
        
        public DateTime BillingPeriodEnd { get; set; }
        
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        
        public DateTime? PaidDate { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; } = null!;
    }
}
