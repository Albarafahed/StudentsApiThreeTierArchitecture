# 🎓 Student Management API

A Student Management API built with C# and .NET, designed to manage student information through a structured multi-layer architecture.

The project separates API responsibilities, business logic, and data access operations into independent layers to improve maintainability, organization, and separation of concerns.

---

## 🚀 Features

### 👨‍🎓 Student Management

- Retrieve student records.
- Retrieve student details.
- Add new students.
- Update student information.
- Delete student records.

*The feature list should be adjusted to match the implemented API endpoints.*

---

## 🏛️ Architecture

The solution follows a layered architecture that separates the application into three main layers.

### Architecture Overview

```text
             +-----------------------------+
             |          StudentApi         |
             |                             |
             |       API Controllers       |
             |       HTTP Endpoints        |
             +-------------+---------------+
                           |
                           ▼
             +-----------------------------+
             |   StudentAPIBusinessLayer   |
             |                             |
             |       Business Logic       |
             |       Application Rules    |
             +-------------+---------------+
                           |
                           ▼
             +-----------------------------+
             |    StudentDataAccessLayer   |
             |                             |
             |       Data Access           |
             |       Database Operations   |
             +-------------+---------------+
                           |
                           ▼
             +-----------------------------+
             |          StudentsDB         |
             |                             |
             |        SQL Database         |
             +-----------------------------+
```

### Solution Projects

| Project | Responsibility |
|---|---|
| **StudentApi** | Handles HTTP requests and API endpoints |
| **StudentAPIBusinessLayer** | Contains business logic and coordinates operations |
| **StudentDataAccessLayer** | Handles database access and data operations |

---

## 🔄 Data Flow

A typical API request follows this flow:

```text
Client
  |
  ▼
StudentApi
  |
  ▼
StudentAPIBusinessLayer
  |
  ▼
StudentDataAccessLayer
  |
  ▼
StudentsDB
```

Each layer has a dedicated responsibility, reducing direct dependencies between the API and database implementation.

---

## 🗄️ Database

The project includes a SQL Server database script:

```text
StudentsDB.sql
```

This script is provided to help create or configure the database required by the application.

---

## 🎬 Project Demo

A demonstration video is included in the repository:

```text
Demo.wmv
```

The video provides a visual demonstration of the application.

---

## ⚙️ Installation

### Requirements

- Windows.
- Visual Studio.
- Compatible .NET SDK.
- Microsoft SQL Server.
- SQL Server Management Studio (SSMS).

### 1. Clone the Repository

```bash
git clone YOUR_REPOSITORY_URL
```

### 2. Open the Solution

Open the solution in Visual Studio.

### 3. Configure the Database

Execute `StudentsDB.sql` using SQL Server Management Studio.

### 4. Configure the Connection String

Update the database connection string according to your SQL Server instance.

### 5. Build and Run

- Build the solution.
- Set `StudentApi` as the Startup Project.
- Run the application.

---

## 📂 Project Structure

```text
StudentManagement
│
├── StudentApi
│
├── StudentAPIBusinessLayer
│
├── StudentDataAccessLayer
│
├── .gitignore
│
├── Demo.wmv
│
├── StudentsDB.sql
│
└── README.md
```

---

## 📚 Learning Objectives

This project provides practical experience with:

- C#.
- .NET.
- ASP.NET Core Web API.
- Layered Architecture.
- Separation of Concerns.
- Business Logic Organization.
- Data Access Layer.
- SQL Server.
- Database Integration.
- RESTful API Development.

---

## 👤 Author

**Albara Fahed Alharissy**

.NET Backend Developer

- GitHub: https://github.com/Albarafahed
- LinkedIn: https://www.linkedin.com/in/albara-csharp-developer/

---

## 📄 License

No license file is currently included in this repository.