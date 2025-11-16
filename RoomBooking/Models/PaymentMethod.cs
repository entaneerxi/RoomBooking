using System.ComponentModel.DataAnnotations;

namespace RoomBooking.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        public PaymentMethodType Type { get; set; }
        
        public string? AccountNumber { get; set; }
        
        public string? AccountName { get; set; }
        
        public string? BankName { get; set; }
        
        public string? QRCodeImageUrl { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
