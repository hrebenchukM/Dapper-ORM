CREATE DATABASE SalesDB; 
GO

USE SalesDB; 
GO



CREATE TABLE customers (
    id INT IDENTITY PRIMARY KEY,
    name NVARCHAR(255),
    date_birth DATETIME ,
    gender NVARCHAR(255) ,
    email NVARCHAR(255) NOT NULL,
    country NVARCHAR(255) ,
    city NVARCHAR(255) 
);

CREATE TABLE categories (
    id INT IDENTITY PRIMARY KEY,
    name NVARCHAR(255) NOT NULL 
);

CREATE TABLE customers_categories (
    id INT IDENTITY PRIMARY KEY,
    customer_id INT ,
    category_id INT ,
    FOREIGN KEY (customer_id) REFERENCES customers(id) ON DELETE CASCADE,
    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE CASCADE
);


CREATE TABLE sales  (
    id INT IDENTITY PRIMARY KEY,
	category_id INT,
    name NVARCHAR(255),
    date_start DATETIME NOT NULL,
    date_end DATETIME NOT NULL,
    country NVARCHAR(255),
    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE CASCADE
);

CREATE TABLE products (
    id INT IDENTITY PRIMARY KEY,
	sale_id INT,
    name NVARCHAR(255) NOT NULL,
    price DECIMAL(10, 2) NOT NULL,
    FOREIGN KEY (sale_id) REFERENCES sales(id) ON DELETE CASCADE
);



	INSERT INTO customers (name, date_birth, gender, email, country, city)
VALUES 
('Mary Hrebenchuk', '2004-12-10', 'Female', 'mary@gmail.com', 'Ukraine', 'Odesa'),
('Iryna Zaiats', '1987-05-10', 'Female', 'iryna@gmail.com', 'Ukraine', 'Odesa'),
('Misha Zaiats', '1987-03-30', 'Male', 'misha@gmail.com', 'Ukraine', 'Odesa'),
('Oleg Semenuk', '1992-10-05', 'Male', 'oleg@gmail.com', 'Ukraine', 'Odesa');


	INSERT INTO categories (name)
VALUES 
('Electronics'),
('Clothing'),
('Home'),
('Books');


	INSERT INTO sales (category_id, name, date_start, date_end, country)
VALUES 
(1, 'Black Friday Electronics Sale', '2025-11-20', '2025-11-27', 'Poland'),
(2, 'Spring Clothing Sale', '2025-12-01', '2025-12-15', 'Canada'),
(3, 'Home Discount', '2025-02-25', '2025-02-26', 'China'),
(4, 'Holiday Book Sale', '2025-03-05', '2025-03-20', 'Ukraine');

	INSERT INTO products (sale_id, name, price)
VALUES 
(1, 'Laptop', 39999.99),
(1, 'Smartphone', 3999.99),
(2, 'Winter Coat', 489.99),
(2, 'Sweater', 149.99),
(3, 'Washing Machine', 2999.99),
(3, 'Microwave', 999.99),
(4, 'Harry Potter', 155.99),
(4, 'C# Book', 255.50);


	INSERT INTO customers_categories (customer_id, category_id)
VALUES 
(1, 1),  
(2, 2),  
(3, 3),  
(4, 4); 



