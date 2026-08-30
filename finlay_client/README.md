# Finlay PharmaVigilance Client

This folder currently contains a .NET console client used for testing, data seeding, and API interaction with the Finlay Pharmacovigilance backend.

Important: this is not yet a browser-based frontend application. It is a local utility client for operational tasks, database initialization, and backend validation.

---

## Purpose

The client is intended to support:

- seeding initial catalog data
- creating sample administrative or test users
- populating vaccination and symptom reference data
- verifying backend API behavior during development
- exercising the application without needing a full UI layer yet

---

## How it works

The client calls the backend API exposed by the ASP.NET project and performs administrative or test actions.

Typical flow:

```text
Start backend API
    ↓
Run the client utility
    ↓
Perform seed or admin actions
    ↓
Validate data in the database and API responses
```

---

## Requirements

Before running the client, make sure the API is available:

- .NET 8 SDK
- backend service running locally
- MySQL / RabbitMQ infrastructure started if required by the project

---

## Run the backend first

```bash
cd finlay_backend
dotnet run --project .\Finlay.PharmaVigilance.API\Finlay.PharmaVigilance.API.csproj
```

The API typically runs on:

```text
http://localhost:5137
```

---

## Run the client

From the repository root:

```bash
cd finlay_client
dotnet run --project Finlay.PharmaVigilance.Client.csproj
```

---

## Seed commands

The project includes a seeding flow for test data. A common example is:

```bash
cd finlay_client
dotnet run --project Finlay.PharmaVigilance.Client.csproj -- seed
```

If your API is running on another base URL, pass it explicitly:

```bash
dotnet run --project Finlay.PharmaVigilance.Client.csproj -- seed http://localhost:5137/api
```

The default base URL is assumed to be:

```text
http://localhost:5137/api
```

You can also pass a custom base URL as the first argument:

```bash
dotnet run --project Finlay.PharmaVigilance.Client.csproj -- "http://localhost:5137/api"
```

---

## Current status

This repository currently has:

- backend API: implemented
- client utility: implemented for testing / seeding
- web frontend: not yet present as a dedicated browser application in this folder

If a real frontend is added later, it should sit in a separate web project, such as:

```text
finlay_frontend/
```

with technologies such as React, Angular, Blazor, or another web stack.

---

## Recommended next step

For a production-ready user interface, the project should eventually include a dedicated frontend application for:

- public reporting forms
- medical review dashboards
- user authentication and profile management
- alerts and case tracking
- administrative reporting panels

If you want, I can prepare the README for that future frontend as well, with a structure ready for React or Blazor.
