using System.ComponentModel.DataAnnotations;
using RoomBooking.Models;

namespace RoomBooking.ViewModels
{
    public class BookingViewModel
    {
        public int RoomId { get; set; }
        
        public Room? Room { get; set; }

        [Required]
        [Display(Name = "Booking Type")]
        public BookingType BookingType { get; set; }

        [Required]
        [Display(Name = "Check-In Date")]
        [DataType(DataType.Date)]
        public DateTime CheckInDate { get; set; }

        [Required]
        [Display(Name = "Check-Out Date")]
        [DataType(DataType.Date)]
        public DateTime CheckOutDate { get; set; }

        [Required]
        [Display(Name = "Number of Guests")]
        [Range(1, 20)]
        public int NumberOfGuests { get; set; } = 1;

        [Display(Name = "Special Requests")]
        [StringLength(500)]
        public string? SpecialRequests { get; set; }

        [Display(Name = "Promo Code")]
        public string? PromoCode { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
    }
}
