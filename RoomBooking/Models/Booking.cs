using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomBooking.Models
{
    public class Booking
    {
        public int Id { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public int RoomId { get; set; }
        
        [Required]
        public BookingType BookingType { get; set; }
        
        [Required]
        public DateTime CheckInDate { get; set; }
        
        [Required]
        public DateTime CheckOutDate { get; set; }
        
        public int NumberOfGuests { get; set; }
        
        [Required]
        public decimal TotalAmount { get; set; }
        
        public decimal? DiscountAmount { get; set; }
        
        public decimal FinalAmount { get; set; }
        
        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        
        public string? SpecialRequests { get; set; }
        
        public string? CancellationReason { get; set; }
        
        public DateTime? PostponeRequestedDate { get; set; }
        
        public DateTime? NewCheckInDate { get; set; }
        
        public DateTime? NewCheckOutDate { get; set; }
        
        public string? PostponeReason { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public DateTime? CheckedInAt { get; set; }
        
        public DateTime? CheckedOutAt { get; set; }
        
        // Navigation properties
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
        
        [ForeignKey("RoomId")]
        public Room Room { get; set; } = null!;
        
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
        
        public MonthlyRental? MonthlyRental { get; set; }
    }
}
