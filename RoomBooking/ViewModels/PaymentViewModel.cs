using System.ComponentModel.DataAnnotations;
using RoomBooking.Models;

namespace RoomBooking.ViewModels
{
    public class PaymentViewModel
    {
        public int BookingId { get; set; }
        
        public Booking? Booking { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public int PaymentMethodId { get; set; }

        public List<PaymentMethod>? PaymentMethods { get; set; }

        [Display(Name = "Transaction Reference")]
        public string? TransactionReference { get; set; }

        [Display(Name = "Payment Proof")]
        public IFormFile? PaymentProof { get; set; }

        [Display(Name = "Notes")]
        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
