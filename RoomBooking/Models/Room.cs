using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models
{
    public class Room
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string RoomNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        [Required]
        public string RoomType { get; set; } = string.Empty;
        
        public int Capacity { get; set; }
        
        [Required]
        public decimal DailyRate { get; set; }
        
        [Required]
        public decimal MonthlyRate { get; set; }
        
        public int FloorNumber { get; set; }
        
        public double? AreaSqm { get; set; }
        
        public RoomStatus Status { get; set; } = RoomStatus.Available;
        
        public string? ImageUrl { get; set; }
        
        public string? Amenities { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
