CREATE TABLE Customers (
    CustomerID AUTOINCREMENT PRIMARY KEY,
    Name TEXT(100),
    Phone TEXT(20),
    LicenceNumber TEXT(50)
);

CREATE TABLE Vehicles (
    VehicleID AUTOINCREMENT PRIMARY KEY,
    VehicleName TEXT(100),
    Category TEXT(50),
    PerDayRate DOUBLE,
    Status TEXT(20) DEFAULT 'Available'
);

CREATE TABLE Rentals (
    RentalID AUTOINCREMENT PRIMARY KEY,
    CustomerID LONG,
    VehicleID LONG,
    RentDate DATE,
    ReturnDate DATE,
    TotalAmount DOUBLE,
    PaymentStatus TEXT(20) DEFAULT 'Pending',
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (VehicleID) REFERENCES Vehicles(VehicleID)
);

CREATE TABLE Maintenance (
    ExpenseID AUTOINCREMENT PRIMARY KEY,
    VehicleID LONG,
    ExpenseDate DATE,
    AmountSpent DOUBLE,
    Description TEXT(255),
    FOREIGN KEY (VehicleID) REFERENCES Vehicles(VehicleID)
);

CREATE TABLE Users (
    UserID AUTOINCREMENT PRIMARY KEY,
    Username TEXT(50) UNIQUE,
    Password TEXT(255),
    StaffName TEXT(100)
);

INSERT INTO Users (Username, Password, StaffName) VALUES ('admin', 'admin123', 'Administrator');
