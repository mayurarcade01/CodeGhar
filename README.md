# Web Technologies with .NET
### Mini Project & Technical Poster Submission

---

**Student Name:** GIRASE MAYUR  
**Enrollment No:** 202403103510603  
**Division:** 5F  
**Institute:** Asha M. Tarsadia Institute of Computer Science & Technology (AMTICS), UTU  
**Submission Date:** 29th October 2026  

---

# 🚀 Project Title: CodeGhar
> **A Calm, Dependable Real-World Source Code Versioning & Management System built with ASP.NET Core MVC and MySQL Entity Framework Core.**

---

## 📌 Project Overview & Problem Statement
Developers frequently work on multiple iterations of code snippets, configuration files, and scripts without wanting the heavy overhead of configuring full remote Git repositories for quick prototypes. 

**CodeGhar** solves this by providing a lightweight, browser-based source code workspace with automated semantic versioning (`v1.0`, `v1.1`, `v2.0`), side-by-side diff comparisons, and instant rollback capabilities, backed by an ACID-compliant relational database.

---

## 📑 Project Deliverables (Images & Technical Poster)

All project screenshots and the official A2 technical presentation poster are organized inside the **`Images & Poster`** folder:

| File Type | File Name | Description / Preview Link |
| :--- | :--- | :--- |
| 📄 **Technical Poster (A2)** | `Poster-A2_CodeGhar_AMTICS_UTU.pdf` | [Open A2 Technical Poster PDF](<./Images & Poster/Poster-A2_CodeGhar_AMTICS_UTU.pdf>) |
| 🖼️ **Screenshot 1** | `dashboard.png` | [View Dashboard Screenshot](<./Images & Poster/dashboard.png>) |
| 🖼️ **Screenshot 2** | `editor.png` | [View Code Editor Screenshot](<./Images & Poster/editor.png>) |
| 🖼️ **Screenshot 3** | `version_difference.png` | [View Version Diff Comparison Screenshot](<./Images & Poster/version_difference.png>) |

---

## 📸 Application Screenshots Preview

### 1. Workspace Dashboard (`dashboard.png`)
*Displays all tracked repository files, language badges, last updated timestamps, version tags, and instant search/filtering.*

![Dashboard](./Images%20&%20Poster/dashboard.png)

---

### 2. Live Code Editor & Version Commit (`editor.png`)
*In-browser source code editing with syntax detection, file renaming, commit messages, and major/minor version increments.*

![Code Editor](./Images%20&%20Poster/editor.png)

---

### 3. Version Difference & Visual Diff Inspector (`version_difference.png`)
*Visual line-by-line diff comparing any two versions with additions, deletions, and instant version rollback/restore.*

![Version Difference](./Images%20&%20Poster/version_difference.png)

---

## 🛠️ Technology Stack
* **Framework:** ASP.NET Core 10.0 (MVC Architecture)
* **Language:** C# 13 / .NET SDK
* **Database & ORM:** MySQL 8.0 with Entity Framework Core (`Pomelo.EntityFrameworkCore.MySql`) + SQLite local fallback
* **Front-End:** Razor Views (`.cshtml`), Modern Flexbox/Grid CSS, JetBrains Mono & Inter typography, HTML5 Dialog Modals
* **State Management:** ASP.NET Core `Session` State (`ISession`), `TempData` across redirects, Memory Cache

---

## 🎯 Key Modules & Syllabus Evaluation Criteria

| Requirement | Implementation in CodeGhar | Key Files |
| :--- | :--- | :--- |
| **MVC Architecture** | Separation of Models, Views, and Controllers. | `Controllers/FilesController.cs`, `Models/CodeFile.cs`, `Views/Files/` |
| **MySQL EF Core** | Relational mapping for `CodeFiles` and `CodeVersions` with Cascade Delete and Foreign Keys. | `Data/ApplicationDbContext.cs`, `Program.cs`, `appsettings.json` |
| **CRUD Operations** | **Create** (new files, upload), **Read** (list, inspect, download), **Update** (save version, rename, restore), **Delete** (soft/hard delete). | `Services/FileService.cs`, `Controllers/FilesController.cs` |
| **Form Handling & Validation** | DataAnnotations (`[Required]`, `[StringLength]`, `[RegularExpression]`) + anti-forgery token validation (`[ValidateAntiForgeryToken]`). | `Models/CodeFile.cs`, `Models/CodeVersion.cs` |
| **State Management** | Session state to track user navigation and file activity + TempData for user notifications. | `Program.cs` (`AddSession`, `UseSession`), `_Layout.cshtml` |
| **Responsive UI** | Clean, desktop and mobile responsive interface with modal forms and accessible tables. | `Views/Shared/_Layout.cshtml`, `wwwroot/css/site.css` |

---

## ⚙️ How to Setup and Run Locally

### Prerequisites
* [.NET 10.0 or 8.0 SDK](https://dotnet.microsoft.com/)
* [MySQL Server / Workbench](https://www.mysql.com/) or XAMPP (optional, project auto-detects local fallback)

### Step 1: Clone Repository
```bash
git clone <your-repository-url>
cd CodeGhar
```

### Step 2: Configure Database (MySQL)
Open `appsettings.json` and adjust your MySQL credentials if necessary:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=CodeGharDB;User=root;Password=root;"
}
```
*(Optional: Run the provided `CodeGhar_MySQL_Setup.sql` in MySQL Workbench to initialize the schema with 5 sample rows).*

### Step 3: Run the Application
```bash
dotnet restore
dotnet run
```

### Step 4: Access in Browser
Open your browser and navigate to:
```
http://localhost:5000
```
*(Application is pre-configured to run cleanly on HTTP port `5000` without SSL certificate warnings).*

---

## 🏛️ Academic Assessment Mapping (Total: 24 Marks)
* **Project Development & Demonstration (16 Marks):** Full MVC implementation, MySQL database integration, CRUD functionality, state management, and live demonstration.
* **Poster Presentation (8 Marks):** High-resolution A2 technical poster (`Poster-A2_CodeGhar_AMTICS_UTU.pdf`) describing system architecture, objectives, technologies, and results.

