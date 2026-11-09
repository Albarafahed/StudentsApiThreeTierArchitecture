# Student Management API

A RESTful API for managing student records, built with ASP.NET Core and .NET 8. The project follows a layered architecture and uses Microsoft SQL Server for data storage, with JWT authentication and role-based and ownership-based authorization.

## Features

- **Student Management:** Create, retrieve, update, and delete student records.
- **Student Statistics:** Retrieve passed students and calculate the average grade.
- **Authentication:** User login with Access Token and Refresh Token generation, token renewal, and logout.
- **Authorization:** Secure endpoints using JWT Bearer authentication, with `Admin` and `Student` roles.
- **Ownership-Based Authorization:** Restrict students to accessing their own records.
- **Rate Limiting:** Limit login attempts to 5 requests per minute per IP address.
- **API Documentation:** OpenAPI documentation through Swagger in the development environment.
- **Automated Testing:** Unit and integration tests using xUnit and Moq.

## Technologies

- **Language:** C#
- **Framework:** ASP.NET Core Web API (.NET 8)
- **Database:** Microsoft SQL Server
- **Data Access:** ADO.NET and Stored Procedures
- **Authentication:** JWT Bearer Authentication
- **Password Hashing:** BCrypt
- **Testing:** xUnit and Moq
- **API Documentation:** Swagger / OpenAPI

## Solution Structure

```text
.
├── StudentApi/                                    # HTTP API, controllers, authentication, and configuration
├── StudentAPIBusinessLayer/                       # Business logic and student services
├── StudentDataAccessLayerStudentDataAccessLayer/  # SQL Server data access
├── Student.Contracts/                             # Shared data transfer objects
├── Stuednt.Test/                                  # Unit and integration tests
├── RateLimitTester/                               # Login rate-limiting test utility
├── StudentsDB.sql                                 # Database schema, stored procedures, and sample data
└── Demo.wmv                                       # Project demonstration video
```

## Prerequisites

- .NET 8 SDK
- Microsoft SQL Server
- SQL Server Management Studio (SSMS) or another tool for executing T-SQL scripts
- Permission to create databases

## Setup and Installation

### 1. Create the Database

Execute the `StudentsDB.sql` script on your SQL Server instance to create the database and its initial schema.

### 2. Configure the Database Connection

Configure the SQL Server connection string in `StudentData.cs` according to your local SQL Server instance before running the application.

### 3. Configure the JWT Secret

Set the `JWT_SECRET_KEY` environment variable to a strong, randomly generated secret containing at least 32 bytes.

Do not commit real secrets to source control.

**PowerShell:**

```powershell
$env:JWT_SECRET_KEY = "your-strong-random-secret-key-at-least-32-bytes"
```

### 4. Run the API

From the solution directory, execute:

```powershell
dotnet run --project StudentApi/StudentApi.csproj
```

The local launch configuration typically uses:

- HTTPS: `https://localhost:7232`
- HTTP: `http://localhost:5155`

In the development environment, access Swagger at:

`https://localhost:7232/swagger`

## Database Schema Compatibility Notice

**Important:** The current application code expects the `Students` table to contain additional authentication-related columns, including:

- `Email`
- `PasswordHash`
- `Role`
- Refresh Token-related fields

However, the provided `StudentsDB.sql` script currently creates the table with only the following columns:

- `Id`
- `Name`
- `Age`
- `Grade`

Additionally, the application calls the following stored procedures, which are not included in the provided SQL script:

- `SP_Login`
- `SP_UpdateRefreshToken`

Therefore, the database schema and stored procedures must be updated to match the application's current data access implementation before authentication and related operations can work correctly.

**The provided SQL script should not be considered fully compatible with the current application code until these differences are resolved.**

## API Endpoints

All endpoints use the `/api` base path.

| Method | Endpoint | Access | Description |
|---|---|---|---|
| `POST` | `/api/Auth/login` | Public (rate-limited) | Authenticate a user and issue tokens |
| `POST` | `/api/Auth/refresh` | Public | Renew tokens using the user's email and refresh token |
| `POST` | `/api/Auth/logout` | Public | Revoke the refresh token |
| `GET` | `/api/Students/Passed` | Public | Retrieve all passed students |
| `GET` | `/api/Students/AverageGrade` | Public | Calculate the average student grade |
| `GET` | `/api/Students/All` | Admin | Retrieve all student records |
| `GET` | `/api/Students/{id}` | Student (own record) or Admin | Retrieve a specific student |
| `POST` | `/api/Students` | Admin | Create a student |
| `PUT` | `/api/Students/{id}` | Admin | Update a student |
| `DELETE` | `/api/Students/{id}` | Admin | Delete a student |

### Authentication Header

For protected endpoints, include the access token in the HTTP request header:

```http
Authorization: Bearer <access-token>
```

## Example Requests

### Login

```http
POST /api/Auth/login
Content-Type: application/json

{
  "email": "student@example.com",
  "password": "your-password"
}
```

### Get Average Grade

```http
GET /api/Students/AverageGrade
```

## Running Tests

To execute the automated tests using the .NET CLI:

```powershell
dotnet test Stuednt.Test/Stuednt.Test.csproj
```

The test project uses:

- **xUnit** for writing and running tests.
- **Moq** for mocking dependencies and isolating application components.

## Security Considerations

- Never commit `JWT_SECRET_KEY` or real credentials to Git.
- Administrative endpoints require the `Admin` role.
- Students can access only their own records through the ownership-based authorization rules.
- Example credentials are placeholders. Use valid accounts after configuring the database.
- Use HTTPS when deploying the API outside a local development environment.

## Demo

The repository includes `Demo.wmv`, a demonstration video showcasing the application.

## Author

**Albara Fahed Alharissy**

- GitHub: [github.com/Albarafahed](https://github.com/Albarafahed)
- LinkedIn: [linkedin.com/in/albara-csharp-developer](https://www.linkedin.com/in/albara-csharp-developer/)

## License

This repository currently does not include a license file. Add a `LICENSE` file to specify the terms under which the project can be used, modified, and distributed.