namespace VehicleRentalApp
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string LicenceNumber { get; set; }
    }

    public class Vehicle
    {
        public int VehicleID { get; set; }
        public string VehicleName { get; set; }
        public string Category { get; set; }
        public decimal PerDayRate { get; set; }
        public string Status { get; set; }
    }

    public class Rental
    {
        public int RentalID { get; set; }
        public int CustomerID { get; set; }
        public int VehicleID { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
    }

    public class Maintenance
    {
        public int ExpenseID { get; set; }
        public int VehicleID { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal AmountSpent { get; set; }
        public string Description { get; set; }
    }

    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string StaffName { get; set; }
    }
}
