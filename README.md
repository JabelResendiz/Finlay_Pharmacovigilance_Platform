# Finlay Pharmacovigilance Platform

<div align="center">
  <img src="finlay_backend/Finlay.PharmaVigilance.API/logo.png" alt="IFV logo" width="220" />
</div>

A digital platform designed for the capture, review, validation, and tracking of adverse events following vaccination, with a strong focus on public health safety, operational traceability, and institutional reporting workflows.

This project was conceived as a practical tool for the management of pharmacovigilance processes, inspired by real-world systems such as VAERS and the Yellow Card Scheme, adapted to the context of the Instituto Finlay de Vacunas (IFV) and institutional health surveillance environments.

---

## Project overview

The system allows citizens, health professionals, and internal reviewers to report potential adverse reactions associated with vaccines, while also enabling specialists to assess, classify, and follow up on cases. The platform supports both public submission and controlled internal review, improving early detection and response to vaccine safety signals.

Main objectives:

- Register suspected adverse events after vaccination
- Support different report sources: citizens, relatives, healthcare professionals
- Enable internal review by medical and administrative staff
- Detect possible duplicates and avoid fragmented records
- Send alerts to responsible health areas when necessary
- Maintain traceability through unique reference numbers
- Facilitate reporting and monitoring in a secure institutional context

---

## Why this project matters

Vaccination is one of the most effective public health interventions, but like any medical intervention, it may be associated with adverse events. Monitoring these events is essential to:

- detect rare or unexpected reactions
- validate vaccine safety signals
- support clinical investigation and data quality
- improve institutional decision-making
- maintain confidence in immunization programs

The platform provides a structured method to collect information consistently and to transform raw reports into actionable evidence for medical and regulatory review.

---

## Promoter and institutional context

This project is associated with the Instituto Finlay de Vacunas (IFV), a Cuban institution with a strong role in vaccine research, development, and public health support.

The platform is conceived as a tool for IFV-related public health and pharmacovigilance workflows, designed to strengthen:

- reporting of post-vaccination adverse events
- interaction with health areas and medical reviewers
- local and institutional monitoring of safety events
- traceability, escalation, and case follow-up

The IFV context is important because the solution is not only a data-entry application; it is meant to support the operational and clinical responsibilities of an institutional surveillance system.

---

## Key capabilities

### Public reporting and intake
- citizens can submit a suspected adverse event without a mandatory pre-registration process
- health professionals can provide more detailed technical clinical information
- reports include vaccine, patient, reporter, and event details

### Role-based review workflows
- medical reviewers validate data and enrich technical information
- section or area responsible users supervise regional workflow
- administrators manage the system and institutional users

### Duplicate detection
- automatic validation of required fields and date consistency
- comparison of key variables such as age, sex, vaccination date, vaccine type, and symptom onset
- specialist review to decide whether reports are duplicate, related, or independent

### Alert and notification mechanisms
- notifications can be sent to the corresponding health area based on the subject location
- priority escalation for severe cases that require urgent follow-up
- email-based communications can be integrated for operational alerts

### PDF support
- the project supports editable PDF forms for offline completion and later upload
- useful for public accessibility and standardized capture

### Security and integrity
- rate limiting and validation controls
- controlled internal user management
- unique reference tracking for each report

---

## Typical user flows

### 1. Citizen or reporter flow

```text
Citizen or healthcare professional
        ↓
Fills in the report form
        ↓
Report is saved in the system
        ↓
Unique reference is generated
        ↓
The case is reviewed by the internal team
```

### 2. Internal medical review flow

```text
Report received
        ↓
Automatic validation
        ↓
Possible duplicate analysis
        ↓
Medical specialist review
        ↓
Case classification and follow-up
```

---

## Repository structure

```text
Finlay_Pharmacovigilance_Platform/
├── README.md
├── finlay_backend/
│   ├── Dockerfile
│   ├── docker-compose.yml
│   ├── README.md
│   ├── Finlay.PharmaVigilance.API/
│   ├── Finlay.PharmaVigilance.Application/
│   ├── Finlay.PharmaVigilance.Domain/
│   ├── Finlay.PharmaVigilance.Infrastructure/
│   └── ...
├── finlay_client/
├── docs/
├── Requests/
├── Templates/
└── test/
```

---

## Technology stack

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- MySQL
- RabbitMQ
- JWT authentication
- Swagger / OpenAPI
- Docker / Docker Compose
- Background services for notifications and operational tasks

---

## Use cases

This platform is intended for:

- public reporting of suspected adverse events after vaccination
- epidemiological monitoring in institutional health programs
- structured review of reports by medical teams
- duplicate case management and case consolidation
- alerting health areas when a serious event is detected
- operational data collection for vaccine safety and signal analysis

---

## Main application flow

1. A user reports a suspected adverse event.
2. The system validates the information and saves the report.
3. A unique report reference is assigned.
4. The case is evaluated for duplicate risk and completeness.
5. A health professional or reviewer enriches the record with clinical context.
6. The event is classified for clinical follow-up, investigation, or closure.
7. Reporting metrics and health alerts can be generated from the data.

---

## Installation guide

### Prerequisites

Before running the project, make sure you have installed:

- .NET 8 SDK
- Docker Desktop or Docker Engine
- Git
- A terminal such as PowerShell, Git Bash, or Bash

### 1. Clone the repository

```bash
git clone https://github.com/your-user/Finlay_Pharmacovigilance_Platform.git
cd Finlay_Pharmacovigilance_Platform
```

### 2. Start infrastructure services with Docker

From the backend folder:

```bash
cd finlay_backend
docker compose up -d
```

This starts:

- MySQL database
- RabbitMQ broker
- API service

### 3. Restore NuGet dependencies

```bash
dotnet restore
```

### 4. Run the API locally

From the backend folder:

```bash
dotnet run --project .\Finlay.PharmaVigilance.API\Finlay.PharmaVigilance.API.csproj
```

The API listens on the port configured in the project, typically `5137`.

### 5. Verify the application

Open the Swagger UI in the browser:

```text
http://localhost:5137/swagger
```

If the application is running correctly, the OpenAPI interface should load and expose the available endpoints.

---

## Docker-based startup

The project includes a Docker Compose configuration for local development and testing.

Commands:

```bash
cd finlay_backend
docker compose up --build
```

To stop services:

```bash
docker compose down
```

To remove volumes when resetting the environment:

```bash
docker compose down -v
```

---

## Environment and configuration

The application configuration is defined in the ASP.NET project settings. Typical values include:

- database connection string
- JWT secret and issuer configuration
- SMTP or email configuration
- RabbitMQ connection settings

The project uses configuration files and environment variables to support different environments such as development and production.

---

## Suggested development workflow

1. Start infrastructure with Docker.
2. Restore and build the .NET solution.
3. Run the API locally.
4. Validate endpoints through Swagger.
5. Apply database migrations if required.
6. Test report creation, duplicate detection, and internal review flows.

---

## Security considerations

This project deals with medical and potentially sensitive information, therefore the following should be treated as essential:

- secure authentication and authorization for internal users
- controlled access to medical review workflows
- careful handling of personal or clinical data
- audit logs and traceability for all important actions
- secure management of environment variables and secrets

---

## Future directions

The platform can continue evolving with:

- better duplicate-matching algorithms
- more advanced dashboards for public health monitoring
- producer and area-level alerts
- integration with notification services and email workers
- PDF processing improvements and validation logic
- more complete traceability and medical case review workflows

---

## License

This project is distributed under the terms defined in the repository license. Please review the license file before reuse or distribution.

---

## Contact and project ownership

This solution is intended for institutional public health and pharmacovigilance use, especially in the context of the Instituto Finlay de Vacunas (IFV). Its design is oriented toward practical safety monitoring, clinical documentation, and operational response.

If you want, I can also create a second version of the README focused on:

- a more executive and institutional presentation for stakeholders
- a technical README for developers
- a lighter GitHub landing-page style README
- a version with badges, screenshots, and a structured architecture section

