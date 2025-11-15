using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models
{
    public class Gallery
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public string ImageUrl { get; set; } = string.Empty;
        
        public string? ThumbnailUrl { get; set; }
        
        public string? Category { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
}
