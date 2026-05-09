<div align="center">

# 🔐 SecureApiArchitecture


[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/license-MIT-green)](LICENSE)
[![Status](https://img.shields.io/badge/status-active-brightgreen)]()

</div>

---

## 📖 Overview

This project focuses on implementing common security mechanisms used in modern APIs, including authentication, authorization, rate limiting, and secure request handling. It is built as a learning and portfolio project to showcase secure backend API development using ASP.NET Core.



## ✨ Features

| Category | Capability |
|---|---|
| **Authentication** | JWT-based authentication |
| **Authorization** | Role-based access control (Admin / User) |
| **Credentials** | Secure password hashing (BCrypt) |
| **Validation** | Request validation |
| **Reliability** | Global exception handling middleware |
| **Observability** | Structured logging via `ILogger` |
| **Abuse Prevention** | API rate limiting |
| **Abuse Prevention** | Basic brute-force login protection |
| **Hardening** | Security headers |
| **Hardening** | HTTPS enforcement |
| **Hardening** | Secure CORS configuration |
| **Docs** | Swagger / OpenAPI documentation |

---

## 🛠 Technologies Used

- ASP.NET Core Web API
- C#
- JWT Authentication
- BCrypt.Net (password hashing)
- ASP.NET Core Rate Limiting
- Swagger / Swashbuckle

---

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- Visual Studio or VS Code

### Clone the repository

```bash
git clone https://github.com/yourusername/SecureApiArchitecture.git
cd SecureApiArchitecture
```

### Run the project

```bash
dotnet restore
dotnet run
```

After running the project, open Swagger in your browser (port may vary — check your console output):

```text
https://localhost:<port>/swagger
```

---

## ⚙️ Configuration

Sensitive and environment-specific settings (JWT secret, token lifetime, CORS allowed origins, rate-limit thresholds, etc.) are managed via `appsettings.json` / `appsettings.Development.json` and environment variables — **not** hardcoded in source.

```text
appsettings.json
└── Jwt:Issuer / Jwt:Audience / Jwt:Key
└── Cors:AllowedOrigins
└── RateLimiting:PermitLimit / Window
```

## 🔌 API Endpoints

All endpoints are documented live via **Swagger / OpenAPI** — this is the single source of truth and always reflects the current code, so it never goes out of sync with this README.

```text
https://localhost:<port>/swagger
```

Endpoints are organized by feature area (e.g. Auth, Account, Admin, Transfer) as separate controllers — explore them directly in Swagger for the full list of routes, request/response schemas, and required roles.

---

## 🔑 Authentication Flow

1. Send a login request to the authentication endpoint.
2. Receive a JWT token.
3. Attach the token to the `Authorization` header of subsequent requests:

```text
Authorization: Bearer <token>
```

4. Access protected endpoints based on your role.

---

## 🛡 Security Overview

This project demonstrates backend security practices commonly used in production APIs:

- **JWT authentication** for secure access control
- **Role-based authorization** for protecting administrative endpoints
- **Password hashing** to securely store user credentials
- **Rate limiting** to prevent API abuse
- **Brute-force protection** for login attempts
- **Security headers** to mitigate common browser-based attacks
- **HTTPS enforcement** for encrypted communication
- **Controlled CORS policy** for trusted origins

---

## 🗺 Roadmap

Planned / possible enhancements:

- [ ] Database integration (Entity Framework Core)
- [ ] Refresh token authentication
- [ ] Persistent account lockout mechanism
- [ ] Audit logging for financial transactions
- [ ] Unit and integration tests
- [ ] Docker containerization


## 🤝 Contributing

This is primarily a learning/portfolio project, but suggestions and pull requests are welcome. Please open an issue first to discuss what you'd like to change.

---

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.

---

## 🎯 Purpose

This project was developed as a learning and portfolio project to demonstrate secure backend API development using ASP.NET Core.
