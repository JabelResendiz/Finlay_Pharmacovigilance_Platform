# Finlay Pharma Platform

[![GitHub license](https://img.shields.io/badge/license-Proprietary-red)](LICENSE)
[![.NET Version](https://img.shields.io/badge/.NET-8.0-blue)](https://dotnet.microsoft.com/)
[![React](https://img.shields.io/badge/React-18.2.0-61DAFB)](https://reactjs.org/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-orange)](https://www.mysql.com/)
[![RabbitMQ](https://img.shields.io/badge/RabbitMQ-3.12-red)](https://www.rabbitmq.com/)

## 📋 Overview

**Finlay Pharma Platform** is a comprehensive pharmacovigilance system designed for the **Finlay Institute**, enabling efficient adverse event reporting, case management, and regulatory compliance. The platform streamlines the entire pharmacovigilance workflow, from initial report submission to medical review and follow-up.

The project is structured as a full-stack solution with a **modular monolith backend** built with ASP.NET Core and a **modern React frontend**, all orchestrated with asynchronous messaging via RabbitMQ.

---

## 🎯 Key Features

- **Adverse Event Reporting**: Intuitive forms for healthcare professionals and patients to submit reports.
- **Case Management**: Complete lifecycle management of pharmacovigilance cases (triage, validation, medical review, follow-up).
- **Real-time Notifications**: Email (SMTP/EmailJS) and WhatsApp alerts for critical events.
- **Role-Based Access Control**: Fine-grained permissions for administrators, medical reviewers, and reporters.
- **Audit Trail**: Full logging of all actions for regulatory compliance.
- **Secure Authentication**: JWT-based authentication with refresh tokens.
- **Bot Protection**: reCAPTCHA and FriendlyCaptcha integration.
- **Asynchronous Processing**: RabbitMQ for background jobs and notifications.

---

## 🧰 Technology Stack

### Backend
| Technology | Purpose |
|------------|---------|
| **.NET 8** | Core framework |
| **ASP.NET Core Web API** | RESTful API endpoints |
| **Entity Framework Core** | ORM for MySQL database |
| **JWT Bearer Authentication** | Secure token-based auth |
| **RabbitMQ** | Message queue for async processing |
| **Serilog** | Structured logging |
| **FluentValidation** | Input validation |
| **AutoMapper** | Object-to-object mapping |
| **xUnit** | Unit testing |
| **Moq** | Mocking framework |

### Frontend
| Technology | Purpose |
|------------|---------|
| **React 18** | UI library |
| **TypeScript** | Type-safe JavaScript |
| **Material-UI** | Component library |
| **React Router** | Client-side routing |
| **Axios** | HTTP client |
| **React Hook Form** | Form management |
| **JWT Decode** | Token handling |

### Infrastructure
| Technology | Purpose |
|------------|---------|
| **MySQL 8.0** | Relational database |
| **RabbitMQ** | Message broker |
| **Docker & Docker Compose** | Containerization |
| **GitHub Actions** | CI/CD pipeline |
| **Prometheus + Grafana** | Monitoring (optional) |

---

## 🚀 Getting Started

### Prerequisites

Make sure you have the following installed on your local development machine:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js & npm](https://nodejs.org/) (v18+ recommended)
- [MySQL Server 8.0+](https://dev.mysql.com/downloads/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for RabbitMQ)
- [Git](https://git-scm.com/)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/) (optional)

---

### 1. Clone the Repository

```bash
git clone https://github.com/JabelResendiz/Finlay_Pharmacovigilance_Platform.git
cd Finlay_Pharmacovigilance_Platform/backend
```

### 2. Backend Setup

### a) Configure 'appsettings.json'

Create the file Finlay.PharmaVigilance.API/appsettings.json with the following structure (replace placeholder values):


```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "AppDbConnectionString": "server=localhost;database=finlay_dev;User=root;Password=YOUR_DB_PASSWORD;"
  },
  "JwtSettings": {
    "Secret": "YOUR_STRONG_SECRET_KEY_AT_LEAST_32_CHARS_LONG",
    "Issuer": "Finlay_PharmaVigilance",
    "ExpiryMinutes": 60,
    "RefreshTokenMinutes": 1440,
    "Audience": "Finlay_PharmaVigilance"
  },
  "Email": {
    "Smtp": {
      "Host": "smtp.gmail.com",
      "Port": 587,
      "User": "your-email@gmail.com",
      "Password": "your-app-password",
      "FromAddress": "noreply@finlay.com",
      "FromName": "Finlay Farmacovigilancia"
    },
    "EmailJS": {
      "ServiceId": "YOUR_SERVICE_ID",
      "UserId": "YOUR_USER_ID",
      "AccessToken": "YOUR_ACCESS_TOKEN",
      "ActivateAccount": "TEMPLATE_ID_1",
      "SelfReportConfirmation": "TEMPLATE_ID_2",
      "AssignmentExpired": "",
      "SectionReportAlert": "",
      "MedicalReviewerAssignment": ""
    }
  },
  "Recaptcha": {
    "SecretKey": "YOUR_RECAPTCHA_SECRET"
  },
  "FriendlyCaptcha": {
    "ApiKey": "YOUR_API_KEY",
    "SiteKey": "YOUR_SITE_KEY"
  },
  "WhatsApp": {
    "ApiBaseUrl": "http://localhost:2785/api",
    "ApiKey": "YOUR_WHATSAPP_API_KEY",
    "SessionId": "YOUR_SESSION_ID",
    "TimeoutSeconds": 30
  }
}
```

### b) Apply Database Migrations

```bash
# Install EF Core tools (if not already installed)
dotnet tool install --global dotnet-ef

# Create and apply migrations
dotnet ef migrations add InitialCreate --project Finlay.PharmaVigilance.Infrastructure --startup-project Finlay.PharmaVigilance.API

dotnet ef database update --project Finlay.PharmaVigilance.Infrastructure --startup-project Finlay.PharmaVigilance.API
```

### c) Run RabbitMQ with Docker

```bash
# First run (pull image and start container)
docker run -d \
  --hostname rabbit \
  --name rabbitmq \
  -p 5672:5672 \
  -p 15672:15672 \
  rabbitmq:3-management

# Or use Docker Compose (if available)
docker-compose up -d
```


### d) Start the Backend API

```bash
dotnet run --project Finlay.PharmaVigilance.API
```

The API will be available at https://localhost:5137. Swagger UI is accessible at /swagger.