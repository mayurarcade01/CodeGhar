-- =====================================================================
-- StudentDB - MySQL Database Setup Script
-- Subject: Web Technologies with .NET - Lab Tutorial
-- =====================================================================

-- 1. Create Database
CREATE DATABASE IF NOT EXISTS StudentDB;
USE StudentDB;

-- 2. Create Table
CREATE TABLE IF NOT EXISTS Students (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    Department VARCHAR(100) NOT NULL
);

-- 3. Insert 5 Sample Rows
INSERT INTO Students (Name, Email, Department) VALUES
('Girase Mayur', 'mayur@example.com', 'Information Technology'),
('Aarav Patel', 'aarav.patel@example.com', 'Computer Science'),
('Priya Sharma', 'priya.sharma@example.com', 'Information Technology'),
('Rohan Mehta', 'rohan.mehta@example.com', 'Software Engineering'),
('Ananya Desai', 'ananya.desai@example.com', 'Data Science');

-- 4. Display Inserted Records
SELECT * FROM Students;

