using Microsoft.AspNetCore.Identity;
using RoomBooking.Models;

namespace RoomBooking.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Create roles
            string[] roleNames = { "Admin", "Staff", "Customer" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Create default admin user
            var adminEmail = "admin@roombooking.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Seed sample payment methods if none exist
            if (!context.PaymentMethods.Any())
            {
                context.PaymentMethods.AddRange(
                    new PaymentMethod
                    {
                        Name = "Cash",
                        Description = "Pay with cash at the property",
                        Type = PaymentMethodType.Cash,
                        IsActive = true,
                        DisplayOrder = 1,
                        CreatedAt = DateTime.UtcNow
                    },
                    new PaymentMethod
                    {
                        Name = "Bank Transfer",
                        Description = "Transfer money to our bank account",
                        Type = PaymentMethodType.BankTransfer,
                        BankName = "Sample Bank",
                        AccountNumber = "1234567890",
                        AccountName = "Room Booking System",
                        IsActive = true,
                        DisplayOrder = 2,
                        CreatedAt = DateTime.UtcNow
                    },
                    new PaymentMethod
                    {
                        Name = "Credit Card",
                        Description = "Pay with credit card",
                        Type = PaymentMethodType.CreditCard,
                        IsActive = true,
                        DisplayOrder = 3,
                        CreatedAt = DateTime.UtcNow
                    }
                );
                await context.SaveChangesAsync();
            }

            // Seed sample rooms if none exist
            if (!context.Rooms.Any())
            {
                context.Rooms.AddRange(
                    new Room
                    {
                        RoomNumber = "101",
                        Name = "Deluxe Room",
                        Description = "A comfortable deluxe room with modern amenities",
                        RoomType = "Deluxe",
                        Capacity = 2,
                        DailyRate = 50.00m,
                        MonthlyRate = 1200.00m,
                        FloorNumber = 1,
                        AreaSqm = 25.0,
                        Status = RoomStatus.Available,
                        Amenities = "WiFi, Air Conditioning, TV, Private Bathroom",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Room
                    {
                        RoomNumber = "102",
                        Name = "Standard Room",
                        Description = "A cozy standard room for budget travelers",
                        RoomType = "Standard",
                        Capacity = 2,
                        DailyRate = 35.00m,
                        MonthlyRate = 800.00m,
                        FloorNumber = 1,
                        AreaSqm = 20.0,
                        Status = RoomStatus.Available,
                        Amenities = "WiFi, Fan, Shared Bathroom",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Room
                    {
                        RoomNumber = "201",
                        Name = "Suite Room",
                        Description = "Spacious suite with premium amenities",
                        RoomType = "Suite",
                        Capacity = 4,
                        DailyRate = 100.00m,
                        MonthlyRate = 2500.00m,
                        FloorNumber = 2,
                        AreaSqm = 45.0,
                        Status = RoomStatus.Available,
                        Amenities = "WiFi, Air Conditioning, TV, Kitchen, Private Bathroom, Balcony",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                );
                await context.SaveChangesAsync();
            }

            // Seed sample facilities if none exist
            if (!context.Facilities.Any())
            {
                context.Facilities.AddRange(
                    new Facility
                    {
                        Name = "Free WiFi",
                        Description = "High-speed wireless internet throughout the property",
                        IconClass = "fa fa-wifi",
                        IsActive = true,
                        DisplayOrder = 1,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Facility
                    {
                        Name = "Parking",
                        Description = "Free parking space for guests",
                        IconClass = "fa fa-car",
                        IsActive = true,
                        DisplayOrder = 2,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Facility
                    {
                        Name = "24/7 Security",
                        Description = "Round-the-clock security for your safety",
                        IconClass = "fa fa-shield",
                        IsActive = true,
                        DisplayOrder = 3,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Facility
                    {
                        Name = "Laundry",
                        Description = "Self-service laundry facilities available",
                        IconClass = "fa fa-tshirt",
                        IsActive = true,
                        DisplayOrder = 4,
                        CreatedAt = DateTime.UtcNow
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}
