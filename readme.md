# Express Firmeza - Employee Management System

## Overview

**Express Firmeza** is a modern, full-stack web application designed for managing employees and related administrative tasks.  
The system integrates **AI services**, **Excel import functionality**, and **PDF generation services**, providing a comprehensive and efficient solution for HR and management operations.

The project follows a **Clean Architecture** approach, separating concerns into layers to ensure maintainability, scalability, and testability.

---

## Features

- **Authentication & Authorization**
  - JWT-based authentication.
  - Role-based access control.
  - Admin user seeded for immediate access.

- **AI Integration**
  - Integration with Gemini AI services for intelligent insights.
  - AI features can be used directly in dashboards and reports.

- **Data Import**
  - Import employee and related data directly from Excel files.
  - Validation ensures correct data before saving.

- **PDF Services**
  - Generate PDF reports of employee data, including summaries and custom filters.
  - Can be triggered from the dashboard.

- **Dashboard**
  - Responsive web interface for administrators.
  - Display employee information, manage users, and access AI services.

---

## Seeded Data

The application comes with an initial admin user to facilitate testing and initial setup:

- **Email:** `admin@expressfirmeza.com`
- **Password:** `Admin@123`

> Make sure to change this password in production for security purposes.

---

## Architecture

The project is structured according to **Clean Architecture principles**:

- **Domain**: Contains entities and core business rules.
- **Application**: Contains DTOs, interfaces, and business logic.
- **Infrastructure**: Handles database, file, PDF, Excel, and AI integrations.
- **Front-end.Web**: ASP.NET Core Razor Pages application that serves as the user interface.
- **API**: Exposes backend functionality and integrates with Frontend and external services.
- **Test_Express**: Unit and integration test project.

This separation ensures that the business logic is independent of frameworks and infrastructure, improving maintainability and scalability.


Running with Docker

The application is fully containerized using Docker Compose:

Build and start all containers:

docker-compose up --build


Services:

PostgreSQL Database: localhost:5432 (mapped from DB_PORT)

API: localhost:5000

Frontend/Admin Dashboard: localhost:5001

Test Service: Runs dotnet test on Test_Express project.

Stopping containers:

docker-compose down

Technical Details

Backend: ASP.NET Core 8.0, Entity Framework Core with PostgreSQL.

Frontend: Razor Pages, responsive design for dashboard.

Testing: Unit and integration tests via Test_Express project.

File Handling: Excel imports and PDF generation integrated using Infrastructure services.

AI Integration: Gemini AI accessible via API key configured in .env.

Authentication: JWT, refresh tokens, email confirmation, password reset.

Seeder Data

The database is pre-populated with:

Admin user: admin@expressfirmeza.com / Admin@123

Sample departments and employee placeholders to test CRUD operations.
