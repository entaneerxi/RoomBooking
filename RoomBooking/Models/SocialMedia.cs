using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models
{
    public class SocialMedia
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Platform { get; set; } = string.Empty;
        
        [Required]
        public string Url { get; set; } = string.Empty;
        
        public string? IconClass { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
