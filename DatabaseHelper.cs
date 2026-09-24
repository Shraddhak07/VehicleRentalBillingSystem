using System;
using System.Data;
using System.Data.OleDb;
using System.IO;

namespace VehicleRentalApp
{
    public static class DatabaseHelper
    {
        private static string ConnString =>
            $"Provider=Microsoft.ACE.OLEDB.19.0;Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "VehicleRental.accdb")};";

        public static OleDbConnection GetOpenConnection()
        {
            var conn = new OleDbConnection(ConnString);
            conn.Open();
            return conn;
        }

        public static DataTable GetTable(string sql)
        {
            using var conn = GetOpenConnection();
            using var cmd = new OleDbCommand(sql, conn);
            using var da = new OleDbDataAdapter(cmd);
            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        public static int ExecuteNonQuery(string sql)
        {
            using var conn = GetOpenConnection();
            using var cmd = new OleDbCommand(sql, conn);
            return cmd.ExecuteNonQuery();
        }

        public static int ExecuteNonQuery(string sql, OleDbConnection conn)
        {
            using var cmd = new OleDbCommand(sql, conn);
            return cmd.ExecuteNonQuery();
        }

        public static object ExecuteScalar(string sql)
        {
            using var conn = GetOpenConnection();
            using var cmd = new OleDbCommand(sql, conn);
            return cmd.ExecuteScalar();
        }

        public static OleDbTransaction BeginTransaction(OleDbConnection conn) => conn.BeginTransaction();

        public static bool ValidateUser(string username, string password, out string staffName)
        {
            staffName = null;
            using var conn = GetOpenConnection();
            using var cmd = new OleDbCommand(
                "SELECT StaffName FROM Users WHERE Username=? AND Password=?", conn);
            cmd.Parameters.AddWithValue("?", username);
            cmd.Parameters.AddWithValue("?", password);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                staffName = reader["StaffName"].ToString();
                return true;
            }
            return false;
        }

        public static void UpdatePassword(string username, string newPassword)
        {
            using var conn = GetOpenConnection();
            using var cmd = new OleDbCommand(
                "UPDATE Users SET Password=? WHERE Username=?", conn);
            cmd.Parameters.AddWithValue("?", newPassword);
            cmd.Parameters.AddWithValue("?", username);
            cmd.ExecuteNonQuery();
        }

        public static DataTable GetAllCustomers() =>
            GetTable("SELECT CustomerID, Name, Phone, LicenceNumber FROM Customers ORDER BY Name");

        public static DataTable GetAllVehicles() =>
            GetTable("SELECT VehicleID, VehicleName, Category, PerDayRate, Status FROM Vehicles ORDER BY VehicleName");

        public static DataTable GetAvailableVehicles() =>
            GetTable("SELECT VehicleID, VehicleName, Category, PerDayRate, Status FROM Vehicles WHERE Status='Available' ORDER BY VehicleName");

        public static DataTable GetVehicleById(int vehicleId) =>
            GetTable($"SELECT * FROM Vehicles WHERE VehicleID={vehicleId}");

        public static DataTable GetActiveRentals() =>
            GetTable(
                "SELECT r.RentalID, c.Name AS CustomerName, v.VehicleName, r.RentDate, r.ReturnDate, " +
                "v.PerDayRate, r.TotalAmount, r.PaymentStatus " +
                "FROM Rentals r " +
                "JOIN Customers c ON r.CustomerID=c.CustomerID " +
                "JOIN Vehicles v ON r.VehicleID=v.VehicleID " +
                "WHERE r.PaymentStatus='Pending' ORDER BY r.RentalID");

        public static DataTable GetRentalHistory() =>
            GetTable(
                "SELECT r.RentalID, c.Name AS CustomerName, v.VehicleName, r.RentDate, r.ReturnDate, " +
                "r.TotalAmount, r.PaymentStatus " +
                "FROM Rentals r " +
                "JOIN Customers c ON r.CustomerID=c.CustomerID " +
                "JOIN Vehicles v ON r.VehicleID=v.VehicleID " +
                "ORDER BY r.RentalID DESC");

        public static DataTable GetRevenueReport(DateTime start, DateTime end) =>
            GetTable(
                "SELECT RentalID, RentDate, TotalAmount " +
                "FROM Rentals " +
                "WHERE PaymentStatus='Paid' AND RentDate BETWEEN ? AND ? ORDER BY RentDate");

        public static DataTable GetExpenseReport(DateTime start, DateTime end) =>
            GetTable(
                "SELECT ExpenseID, VehicleID, ExpenseDate, AmountSpent, Description " +
                "FROM Maintenance " +
                "WHERE ExpenseDate BETWEEN ? AND ? ORDER BY ExpenseDate");

        public static DataTable GetVehicleAvailability() =>
            GetTable("SELECT Status, COUNT(*) AS Count FROM Vehicles GROUP BY Status");
    }
}
