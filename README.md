# 🔐 JWT Authentication & Authorization API

## 📌 Project Overview

This project is a secure **Authentication & Authorization API** developed using **ASP.NET Core Web API (.NET 8)** following the **N-Tier Architecture**.

The API provides user authentication, role-based authorization, JWT Access Token generation, Refresh Token management, and Client Credentials authentication.

---

## 🚀 Features

* 🔐 JWT Authentication
* 🔄 Refresh Token Mechanism
* 👤 User Registration
* 🔑 User Login
* 🛡 Role-Based Authorization
* 🤖 Client Credentials Flow
* 🔁 Refresh Token Revocation
* 📦 Standard API Response
* ⚠ Global Exception Handling
* ✅ Custom Validation Responses
* 🔄 Entity ↔ DTO Mapping (AutoMapper)
* 🏗 Generic Repository Pattern
* 🏗 Unit of Work Pattern
* 💉 Dependency Injection

---

## 🛠 Technologies

### Backend

* ASP.NET Core Web API (.NET 8)
* ASP.NET Core Identity
* Entity Framework Core
* SQL Server
* JWT Authentication
* Refresh Token
* AutoMapper
* Fluent API

### Architecture

* N-Tier Architecture
* Generic Repository Pattern
* Unit of Work Pattern
* Dependency Injection

### Tools

* Visual Studio 2022
* Swagger
* Git
* GitHub

---

## 📂 Project Structure

```text
UdemyAuthServer.API
│
├── Controllers
├── Models
├── Validation
├── Properties
├── wwwroot
├── Program.cs
├── appsettings.json
└── UdemyAuthServer.API.sln
```

---

## 🔄 Authentication Flow

1. User registers an account.
2. User logs in with credentials.
3. A JWT Access Token and Refresh Token are generated.
4. When the Access Token expires, a new Access Token is issued using the Refresh Token.
5. Refresh Tokens can be revoked for enhanced security.

---

## 📡 API Endpoints

| Method | Endpoint                           | Description                       |
| ------ | ---------------------------------- | --------------------------------- |
| POST   | `/api/auth/register`               | Register a new user               |
| POST   | `/api/auth/login`                  | Authenticate a user               |
| POST   | `/api/auth/create-token-by-client` | Client Credentials authentication |
| POST   | `/api/auth/refresh-token`          | Generate a new Access Token       |
| POST   | `/api/auth/revoke-refresh-token`   | Revoke Refresh Token              |

---

## ⚙️ Getting Started

```bash
git clone https://github.com/gkcdgn/jwt-authentication-api.git
```

```bash
cd jwt-authentication-api
```

1. Configure the SQL Server connection string in `appsettings.json`.
2. Apply the database migrations.
3. Run the project.
4. Open Swagger and test the API endpoints.

---

## 👨‍💻 Author

**Gökçe Doğan**
