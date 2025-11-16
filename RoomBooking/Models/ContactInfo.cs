using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string PropertyName { get; set; } = string.Empty;
        
        [Required]
        public string Address { get; set; } = string.Empty;
        
        [Phone]
        public string? Phone { get; set; }
        
        [EmailAddress]
        public string? Email { get; set; }
        
        public string? Website { get; set; }
        
        public string? MapEmbedUrl { get; set; }
        
        public string? Description { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
}
