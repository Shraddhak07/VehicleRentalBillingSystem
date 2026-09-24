CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100),
    Phone NVARCHAR(20),
    LicenceNumber NVARCHAR(50)
);

CREATE TABLE Vehicles (
    VehicleID INT IDENTITY(1,1) PRIMARY KEY,
    VehicleName NVARCHAR(100),
    Category NVARCHAR(50),
    PerDayRate DECIMAL(18,2),
    Status NVARCHAR(20) DEFAULT 'Available'
);

CREATE TABLE Rentals (
    RentalID INT IDENTITY(1,1) PRIMARY KEY,
    CustomerID INT,
    VehicleID INT,
    RentDate DATE,
    ReturnDate DATE,
    TotalAmount DECIMAL(18,2),
    PaymentStatus NVARCHAR(20) DEFAULT 'Pending',
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID),
    FOREIGN KEY (VehicleID) REFERENCES Vehicles(VehicleID)
);

CREATE TABLE Maintenance (
    ExpenseID INT IDENTITY(1,1) PRIMARY KEY,
    VehicleID INT,
    ExpenseDate DATE,
    AmountSpent DECIMAL(18,2),
    Description NVARCHAR(255),
    FOREIGN KEY (VehicleID) REFERENCES Vehicles(VehicleID)
);

CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE,
    Password NVARCHAR(255),
    StaffName NVARCHAR(100)
);

INSERT INTO Users (Username, Password, StaffName) VALUES ('admin', 'admin123', 'Administrator');
