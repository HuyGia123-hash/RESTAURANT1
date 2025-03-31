-- Tạo cơ sở dữ liệu
CREATE DATABASE qlnhahang;
GO
USE qlnhahang;
GO

-- Bảng Categories
CREATE TABLE Categories (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

-- Bảng MenuItems
CREATE TABLE MenuItems (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Price DECIMAL(10, 2) NOT NULL,
    Description NVARCHAR(MAX),
    CategoryId INT,
    IsAvailable BIT DEFAULT 1,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id)
);

-- Bảng Tables (Bàn ăn)
CREATE TABLE Tables (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TableName NVARCHAR(100) NOT NULL,
    Capacity INT,
    IsAvailable BIT DEFAULT 1
);

-- Bảng Customers
CREATE TABLE Customers (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CustomerCode NVARCHAR(50) UNIQUE,
    Name NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20) UNIQUE
);

-- Bảng Reservations (Đặt bàn)
CREATE TABLE Reservations (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT,
    ReservationDate DATE,
    ArrivalDate DATETIME,
    NumberOfGuests INT,
    Status NVARCHAR(50),
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);

-- Bảng Users (Nhân viên và quản lý)
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) UNIQUE,
    PhoneNumber NVARCHAR(20) UNIQUE,
    PasswordHash NVARCHAR(255),
    Role NVARCHAR(50),
    RoleId INT
);

-- Bảng Orders (Đơn hàng)
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CustomerId INT,
    OrderDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);

-- Bảng OrderDetails (Chi tiết đơn hàng)
CREATE TABLE OrderDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT,
    MenuItemId INT,
    Quantity INT NOT NULL,
    PriceAtOrder DECIMAL(10, 2) NOT NULL,
    SubTotal AS (Quantity * PriceAtOrder) PERSISTED,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id),
    FOREIGN KEY (MenuItemId) REFERENCES MenuItems(Id)
);

-- Bảng Notifications (Thông báo)
CREATE TABLE Notifications (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT,
    Message NVARCHAR(MAX),
    IsRead BIT DEFAULT 0,
    FOREIGN KEY (OrderId) REFERENCES Orders(Id)
);

-- Thêm danh mục món ăn
INSERT INTO Categories (Name) VALUES 
(N'Appetizers'),
(N'Main Courses'),
(N'Desserts'),
(N'Beverages');

-- Thêm món ăn vào menu
INSERT INTO MenuItems (Name, Price, Description, CategoryId, IsAvailable) VALUES
(N'Spring Rolls', 5.99, N'Crispy fried spring rolls with sweet chili sauce', 1, 1),
(N'Grilled Chicken', 12.99, N'Served with roasted vegetables', 2, 1),
(N'Chocolate Cake', 6.50, N'Rich chocolate cake with vanilla ice cream', 3, 1),
(N'Iced Coffee', 3.99, N'Vietnamese iced coffee with condensed milk', 4, 1);

-- Thêm bàn ăn
INSERT INTO Tables (TableName, Capacity, IsAvailable) VALUES
(N'Table 1', 4, 1),
(N'Table 2', 6, 1),
(N'Table 3', 2, 1),
(N'Table 4', 8, 1);

-- Thêm khách hàng
INSERT INTO Customers (CustomerCode, Name, PhoneNumber) VALUES
(N'CUST001', N'Nguyen Van A', N'0987654321'),
(N'CUST002', N'Tran Thi B', N'0912345678'),
(N'CUST003', N'Le Van C', N'0909876543');

-- Thêm đặt bàn
INSERT INTO Reservations (CustomerId, ReservationDate, ArrivalDate, NumberOfGuests, Status) VALUES
(1, '2025-04-01', '2025-04-01 18:30:00', 4, N'Confirmed'),
(2, '2025-04-02', '2025-04-02 19:00:00', 2, N'Pending'),
(3, '2025-04-03', '2025-04-03 20:00:00', 6, N'Confirmed');

-- Thêm nhân viên
INSERT INTO Users (FullName, Email, PhoneNumber, PasswordHash, Role, RoleId) VALUES
(N'Admin User', N'admin@restaurant.com', N'0999999999', 'hashedpassword1', N'Admin', 1),
(N'Waiter A', N'waiterA@restaurant.com', N'0888888888', 'hashedpassword2', N'Waiter', 2),
(N'Cashier B', N'cashierB@restaurant.com', N'0777777777', 'hashedpassword3', N'Cashier', 3);

-- Thêm đơn hàng
INSERT INTO Orders (CustomerId, OrderDate) VALUES
(1, GETDATE()),
(2, GETDATE()),
(3, GETDATE());

-- Thêm chi tiết đơn hàng
INSERT INTO OrderDetails (OrderId, MenuItemId, Quantity, PriceAtOrder) VALUES
(1, 1, 2, 5.99),  -- 2 phần Spring Rolls
(1, 2, 1, 12.99), -- 1 phần Grilled Chicken
(2, 3, 3, 6.50),  -- 3 phần Chocolate Cake
(3, 4, 2, 3.99);  -- 2 ly Iced Coffee

-- Thêm thông báo
INSERT INTO Notifications (OrderId, Message, IsRead) VALUES
(1, N'Your order has been received!', 0),
(2, N'Your order is being prepared.', 0),
(3, N'Your order is ready for pickup.', 0);
