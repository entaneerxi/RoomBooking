namespace RoomBooking.Models
{
    public enum BookingType
    {
        Daily,
        Monthly
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        CheckedIn,
        CheckedOut,
        Cancelled,
        PostponeRequested
    }

    public enum PaymentStatus
    {
        Pending,
        Paid,
        Failed,
        Refunded
    }

    public enum PaymentMethodType
    {
        Cash,
        BankTransfer,
        CreditCard,
        MobileBanking,
        QRCode
    }

    public enum RoomStatus
    {
        Available,
        Occupied,
        Maintenance,
        Reserved
    }

    public enum UserRole
    {
        Customer,
        Staff,
        Admin
    }
}
