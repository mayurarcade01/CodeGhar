-- =====================================================================
-- CodeGhar - MySQL Database Setup Script for Mini Project Evaluation
-- Subject: Web Technologies with .NET (AMTICS / UTU)
-- Student: Girase Mayur
-- =====================================================================

-- Step 1: Create Database
CREATE DATABASE IF NOT EXISTS CodeGharDB 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

USE CodeGharDB;

-- Step 2: Create Tables (EF Core Entity Models)
CREATE TABLE IF NOT EXISTS CodeFiles (
    Id CHAR(36) NOT NULL,
    Name VARCHAR(180) NOT NULL,
    Extension VARCHAR(20) NOT NULL,
    IsDeleted TINYINT(1) NOT NULL DEFAULT 0,
    CreatedUtc DATETIME(6) NOT NULL,
    UpdatedUtc DATETIME(6) NOT NULL,
    CurrentVersionNumber INT NOT NULL DEFAULT 1,
    PRIMARY KEY (Id),
    INDEX IX_CodeFiles_Name_IsDeleted (Name, IsDeleted)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS CodeVersions (
    Id CHAR(36) NOT NULL,
    CodeFileId CHAR(36) NOT NULL,
    Number INT NOT NULL,
    Major INT NOT NULL DEFAULT 1,
    Minor INT NOT NULL DEFAULT 0,
    CreatedUtc DATETIME(6) NOT NULL,
    Message VARCHAR(160) NULL,
    Content LONGTEXT NOT NULL,
    AddedLines INT NOT NULL DEFAULT 0,
    DeletedLines INT NOT NULL DEFAULT 0,
    PRIMARY KEY (Id),
    UNIQUE INDEX IX_CodeVersions_CodeFileId_Number (CodeFileId, Number),
    CONSTRAINT FK_CodeVersions_CodeFiles_CodeFileId 
        FOREIGN KEY (CodeFileId) REFERENCES CodeFiles(Id) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Step 3: Insert 5 Sample Records into CodeFiles
INSERT INTO CodeFiles (Id, Name, Extension, IsDeleted, CreatedUtc, UpdatedUtc, CurrentVersionNumber) VALUES
('b1a10001-0000-0000-0000-000000000001', 'Program.cs', '.cs', 0, NOW(), NOW(), 2),
('b1a10002-0000-0000-0000-000000000002', 'appsettings.json', '.json', 0, NOW(), NOW(), 1),
('b1a10003-0000-0000-0000-000000000003', 'README.md', '.md', 0, NOW(), NOW(), 1),
('b1a10004-0000-0000-0000-000000000004', 'site.css', '.css', 0, NOW(), NOW(), 1),
('b1a10005-0000-0000-0000-000000000005', 'StudentController.cs', '.cs', 0, NOW(), NOW(), 1)
ON DUPLICATE KEY UPDATE UpdatedUtc = NOW();

-- Insert Sample Versions for the Records
INSERT INTO CodeVersions (Id, CodeFileId, Number, Major, Minor, CreatedUtc, Message, Content, AddedLines, DeletedLines) VALUES
('c1a10001-0000-0000-0000-000000000001', 'b1a10001-0000-0000-0000-000000000001', 1, 1, 0, NOW(), 'Initial setup with WebApplication builder', 'var builder = WebApplication.CreateBuilder(args);', 1, 0),
('c1a10001-0000-0000-0000-000000000002', 'b1a10001-0000-0000-0000-000000000001', 2, 2, 0, NOW(), 'Added Pomelo MySQL EF Core and Session State', 'var builder = WebApplication.CreateBuilder(args);\nbuilder.Services.AddSession();', 2, 0),
('c1a10002-0000-0000-0000-000000000001', 'b1a10002-0000-0000-0000-000000000002', 1, 1, 0, NOW(), 'Configured MySQL ConnectionStrings', '{\n  "ConnectionStrings": {\n    "DefaultConnection": "Server=localhost;Port=3306;Database=CodeGharDB;User=root;Password=root;"\n  }\n}', 5, 0),
('c1a10003-0000-0000-0000-000000000001', 'b1a10003-0000-0000-0000-000000000003', 1, 1, 0, NOW(), 'Project documentation', '# CodeGhar\nReal-world source code versioning in ASP.NET Core MVC with MySQL.', 2, 0),
('c1a10004-0000-0000-0000-000000000001', 'b1a10004-0000-0000-0000-000000000004', 1, 1, 0, NOW(), 'Responsive UI styling with flexbox & CSS grid', '/* Responsive styles */\n:root { --blue: #2563eb; }', 2, 0),
('c1a10005-0000-0000-0000-000000000001', 'b1a10005-0000-0000-0000-000000000005', 1, 1, 0, NOW(), 'Student management module controller', 'namespace CodeGhar.Controllers;\npublic class StudentController : Controller {}', 2, 0)
ON DUPLICATE KEY UPDATE CreatedUtc = NOW();

-- Step 4: Display inserted records (Verify in MySQL Workbench)
SELECT 
    f.Id, 
    f.Name, 
    f.Extension, 
    f.CurrentVersionNumber, 
    f.CreatedUtc,
    COUNT(v.Id) AS TotalVersions
FROM CodeFiles f
LEFT JOIN CodeVersions v ON f.Id = v.CodeFileId
WHERE f.IsDeleted = 0
GROUP BY f.Id, f.Name, f.Extension, f.CurrentVersionNumber, f.CreatedUtc;

SELECT * FROM CodeVersions;

