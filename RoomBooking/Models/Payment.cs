using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoomBooking.Models
{
    public class Payment
    {
        public int Id { get; set; }
        
        [Required]
        public int BookingId { get; set; }
        
        [Required]
        public string UserId { get; set; } = string.Empty;
        
        [Required]
        public decimal Amount { get; set; }
        
        [Required]
        public int PaymentMethodId { get; set; }
        
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        
        public string? TransactionReference { get; set; }
        
        public string? PaymentProofUrl { get; set; }
        
        public DateTime? PaidAt { get; set; }
        
        public string? ConfirmedBy { get; set; }
        
        public DateTime? ConfirmedAt { get; set; }
        
        public string? Notes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; } = null!;
        
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; } = null!;
        
        [ForeignKey("PaymentMethodId")]
        public PaymentMethod PaymentMethod { get; set; } = null!;
    }
}
