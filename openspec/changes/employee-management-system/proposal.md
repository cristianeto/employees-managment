## Why

ProCreditGroup's take-home exam requires building an end-to-end Employee Management System — replacing spreadsheet-based HR tracking with a normalized SQL Server database, a layered .NET 10 REST API, and a React 19 SPA — as specified in `docs/Prueba Desarrollador FullStack Digital_v1.1.md`. All three layers currently exist only as bare scaffolds (default templates, no business logic), so this change delivers the full feature set from scratch.

## What Changes

- Add SQL Server schema: normalized `Employee`, `Area`, and `Cargo` (Position) tables with PK/identity columns and FK relationships (Employee → Area, Employee → Cargo), so renaming an area/position updates automatically everywhere.
- Add one stored procedure for querying employees (used by the area/department filter search).
- Add Bearer Token (JWT) authentication to the API, backed by a single preconfigured test user; no self-service registration.
- Add API endpoint to list all employees (with their Area and Cargo names).
- Add API endpoint to create a new employee.
- Add API endpoint to search/filter employees by Area (department).
- Add layered backend implementation: Domain entities, Application use cases/DTOs, Infrastructure (EF Core `DbContext`, configs, migrations, repositories), Api (controllers, JWT auth, DI wiring).
- Add React 19 frontend screens: Login form, employee list table (loads on screen entry), "Add Employee" modal form; wire both to the list-all and filter-by-area endpoints using MUI components and react-query.

## Capabilities

### New Capabilities
- `employee-directory`: Core employee/area/cargo data model and the list-all, create-employee, and filter-by-area operations (spans DB schema + API + frontend list/add screens).
- `auth`: Bearer token authentication with a preconfigured test user, covering the login endpoint, JWT issuance/validation, and the frontend login form.

### Modified Capabilities
(none — greenfield, no existing specs)

## Impact

- **Database**: new DDL under `database/` (tables, FKs, stored procedure), applied to the SQL Server container defined in `docker-compose.yml`.
- **Backend**: all four projects under `backend/src/` gain real implementations — `Domain` (entities), `Application` (use cases, DTOs, auth contracts), `Infrastructure` (EF Core `DbContext`, entity configs, migrations, repository implementations), `Api` (controllers, JWT bearer middleware, DI registration), replacing the current `Class1.cs`/`WeatherForecastController` placeholders.
- **Frontend**: `frontend/src/App.tsx` and related files gain routing (`react-router-dom`), auth state, a login page, an employee list page (table + search-by-area), and an add-employee modal, using `@mui/material` and `@tanstack/react-query`.
- **Dependencies**: no new packages expected — auth (`Microsoft.AspNetCore.Authentication.JwtBearer`), EF Core SQL Server, MUI, react-query, and axios are already referenced per `CLAUDE.md`.
