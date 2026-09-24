# Vehicle Rental & Billing System

A simple Windows Forms application for college projects.

## Stack
- Frontend: C# Windows Forms (.NET 9) with modern dark-blue theme styling
- Backend: Microsoft Access database with `System.Data.OleDb`
- Optional SQL Server DDL scripts are included.
- UI: Themed controls, styled buttons, gradient headers, DataGridView styling

## Files
- `VehicleRentalApp.csproj` - Project file
- `Program.cs` - Application entry point
- `Models.cs` - Data models
- `DatabaseHelper.cs` - Access database helpers
- `frmSplash.cs` - Loading screen
- `frmLogin.cs` - Staff login
- `frmChangePassword.cs` - Password update
- `frmMainMenu.cs` - Navigation hub (MDI parent)
- `frmAddVehicle.cs` / `frmManageVehicles.cs` - Fleet management
- `frmAddCustomer.cs` / `frmManageCustomers.cs` - Customer management
- `frmBookRental.cs` - Book rental and calculate total bill
- `frmReturnVehicle.cs` - Return vehicle, calculate late fees, mark paid
- `frmMaintenanceExpenses.cs` - Maintenance cost logging
- `frmRentalHistory.cs` - Historic transaction list
- `frmRevenueReport.cs` - Revenue metrics
- `frmExpenseReport.cs` - Expense breakdown
- `frmVehicleAvailability.cs` - Available vs rented inventory
- `frmAbout.cs` - Credits
- `scripts/` - Database DDL scripts

## Setup
1. Install .NET 9 SDK if not already installed.
2. Run:
   ```powershell
   dotnet restore
   dotnet build
   ```
3. Create an empty Microsoft Access database file named `VehicleRental.accdb` next to the executable, or adjust the connection string in `DatabaseHelper.cs`.
4. Run `scripts/AccessSchema.sql` to create the tables and default admin user.
5. Default login: `admin` / `admin123`.

## Main Formula
`(ReturnDate - RentDate) * PerDayRate = TotalBill`

## Financial Transactions
- `frmBookRental` inserts a rental and sets vehicle status to `Rented` inside a transaction.
- `frmReturnVehicle` updates the rental, calculates late fees, sets vehicle to `Available`, and marks payment `Paid` inside a transaction.
