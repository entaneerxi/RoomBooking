# Room Booking System

A comprehensive room booking management system built with ASP.NET Core 8, Entity Framework Core, and SQL Server.

## Features

### Frontend (Customer-Facing)
- **Room Browsing**: View available rooms with filtering by type and price
- **Booking System**: 
  - Daily and monthly booking options
  - Real-time availability checking
  - Special requests and guest information
- **User Authentication**: Registration, login, and profile management
- **Booking Management**: 
  - View booking history
  - Request booking postponements
  - Track booking status
- **Payments**: 
  - Multiple payment methods support
  - Payment proof upload
  - Payment history tracking
- **Promotions**: View active promotions and special offers
- **Gallery**: Browse property and room images
- **Facilities**: View available amenities and services

### Backend (Admin Panel)
- **Dashboard**: 
  - Key metrics overview (total rooms, bookings, revenue)
  - Recent bookings summary
  - Today's check-ins/check-outs
- **Booking Management**:
  - Confirm bookings and payments
  - Manage check-in/check-out processes
  - Handle postponement requests
  - View detailed booking information
- **Monthly Rental Management**:
  - Track water and electricity usage
  - Generate utility bills
  - Manage tenant information
- **Payment Management**:
  - Confirm payments
  - View payment proofs
  - Payment history and tracking
- **Reports**:
  - Booking reports (date range)
  - Monthly revenue reports
  - Utilities (water/electricity) reports
  - PDF export functionality using QuestPDF
- **Content Management (CRUD)**:
  - Rooms (add, edit, delete, manage availability)
  - Promotions
  - Payment Methods
  - Facilities
  - Gallery Images
  - Homepage Slides
  - Booking Add-ons
  - Contact Information
  - Social Media Links
- **User Management**:
  - Manage customers, staff, and admin accounts
  - Role assignment (Admin, Staff, Customer)
  - User activation/deactivation

## Technology Stack

### Backend
- **Framework**: ASP.NET Core 8
- **Language**: C# 12
- **ORM**: Entity Framework Core 8
- **Database**: Microsoft SQL Server (LocalDB for development)
- **Authentication**: ASP.NET Core Identity
- **PDF Generation**: QuestPDF

### Frontend
- **UI Framework**: Bootstrap 5
- **JavaScript**: jQuery
- **Notifications**: SweetAlert2
- **Icons**: Font Awesome 6

## Project Structure

```
RoomBooking/
├── Areas/
│   └── Admin/
│       ├── Controllers/         # Admin panel controllers
│       └── Views/              # Admin panel views
├── Controllers/                # Frontend controllers
├── Data/
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs       # Database seeding
├── Models/                     # Entity models
│   ├── ApplicationUser.cs
│   ├── Room.cs
│   ├── Booking.cs
│   ├── Payment.cs
│   ├── Promotion.cs
│   ├── Gallery.cs
│   ├── Facility.cs
│   ├── MonthlyRental.cs
│   └── ...
├── ViewModels/                # Form view models
├── Services/
│   └── ReportService.cs       # PDF report generation
├── Views/                     # Frontend views
└── wwwroot/                   # Static files
```

## Database Models

### Core Entities
- **ApplicationUser**: Extended Identity user with additional properties
- **Room**: Room information, pricing, and availability
- **Booking**: Booking records with daily/monthly types
- **Payment**: Payment transactions and methods
- **MonthlyRental**: Utilities tracking for monthly rentals
- **Promotion**: Promotional offers and discounts
- **Gallery**: Property and room images
- **Facility**: Available amenities
- **ContactInfo**: Property contact information
- **SocialMedia**: Social media links
- **HomepageSlide**: Homepage carousel slides
- **BookingAddon**: Additional booking options
- **PaymentMethod**: Available payment methods

### Enums
- **BookingType**: Daily, Monthly
- **BookingStatus**: Pending, Confirmed, CheckedIn, CheckedOut, Cancelled, PostponeRequested
- **PaymentStatus**: Pending, Paid, Failed, Refunded
- **RoomStatus**: Available, Occupied, Maintenance, Reserved
- **PaymentMethodType**: Cash, BankTransfer, CreditCard, MobileBanking, QRCode

## Getting Started

### Prerequisites
- .NET 8 SDK
- SQL Server or SQL Server Express
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository
2. Update the connection string in `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RoomBookingDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

3. Run database migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

4. Run the application:
```bash
dotnet run
```

5. Default admin credentials:
   - Email: admin@roombooking.com
   - Password: Admin@123

### First-Time Setup

The system will automatically:
- Create required roles (Admin, Staff, Customer)
- Create a default admin user
- Seed sample data (rooms, payment methods, facilities)

## API Endpoints

### Frontend Routes
- `/` - Homepage
- `/Rooms` - Browse rooms
- `/Rooms/Details/{id}` - Room details
- `/Bookings/Create/{roomId}` - Create booking
- `/Bookings/MyBookings` - User's bookings
- `/Payments/Create/{bookingId}` - Make payment
- `/Promotions` - View promotions
- `/Gallery` - View gallery
- `/Facilities` - View facilities
- `/Account/Login` - User login
- `/Account/Register` - User registration

### Admin Routes
- `/Admin/Dashboard` - Admin dashboard
- `/Admin/Bookings` - Manage bookings
- `/Admin/Payments` - Manage payments
- `/Admin/MonthlyRentals` - Manage monthly rentals
- `/Admin/Rooms` - Manage rooms
- `/Admin/Reports` - Generate reports
- `/Admin/Users` - Manage users

## Features Implementation Status

✅ **Completed:**
- Database models and relationships
- Entity Framework Core configuration
- Identity authentication and authorization
- All backend controllers
- PDF report generation
- SweetAlert2 integration
- Admin panel navigation
- Role-based access control
- Database seeding

⏳ **Pending:**
- Frontend views (HTML/Razor)
- Image upload functionality
- Email notifications
- Payment gateway integration
- Advanced search and filtering
- Booking calendar UI
- Dashboard charts and graphs

## Security Features

- Password hashing with Identity
- Role-based authorization (Admin, Staff, Customer)
- Anti-forgery tokens on forms
- SQL injection protection via EF Core
- Secure session management
- User account lockout after failed attempts

## Contributing

This is a complete booking system ready for customization and deployment. Key areas for enhancement:
1. Create Razor views for all controllers
2. Implement file upload for images
3. Add email notification service
4. Integrate payment gateway
5. Add unit and integration tests
6. Implement caching for performance
7. Add API for mobile apps

## License

This project is provided as-is for educational and commercial use.