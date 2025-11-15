using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public string? ImageUrl { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        public decimal? DiscountPercentage { get; set; }
        
        public decimal? DiscountAmount { get; set; }
        
        public string? PromoCode { get; set; }
        
        public string? TermsAndConditions { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
}
