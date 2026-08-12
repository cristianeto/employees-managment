# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this repo is

This is a take-home technical exam for ProCreditGroup (see `docs/Prueba Desarrollador FullStack Digital_v1.1.md`, in Spanish). The task is to build an **Employee Management System** end-to-end: a SQL Server database, a layered .NET 10 REST API, and a React 19 SPA. As of now, all three parts are bare scaffolds (default templates, no business logic yet) — most implementation work is still ahead.

### Business requirements (from the exam brief)

- Employees are uniquely identified by their national ID document number, and have first name, last name, and age.
- Each employee belongs to exactly one **Area** (Recursos Humanos, Finanzas, Contabilidad, Marketing, Sistemas, Banca Empresas, Banca Personas); an area has many employees.
- Each employee holds exactly one **Cargo/Position** (e.g. Analista de RRHH, Contador Senior, Supervisor de Créditos, Diseñador UX/UI, Especialista de Sistemas) at a time; a position can be held by many employees.
- Each employee has a monthly salary (used for payroll/expense reports, averages, cost-by-area analysis).
- The schema must be normalized — renaming an area or position must be a single update, not a per-employee change. This means Area and Cargo/Position are their own tables (FK from Employee), not denormalized strings.

Required deliverables:
- **Database (SQL Server)**: tables with PK/identity columns, FK relationships, and one stored procedure for querying employees.
- **API (C#, .NET 10, layered architecture)**: Bearer token auth with a preconfigured test user; list all employees; add a new employee; search/filter employees by area/department.
- **Frontend (React 19 + a UI component library)**: login form; employee list table; "add employee" modal form; wire up the list-all endpoint (loads on screen entry) and the department-filter search endpoint.

## Repo layout

```
backend/    .NET 10 solution (layered architecture)
frontend/   React 19 + Vite + TypeScript SPA
database/   intended for SQL Server schema/scripts — currently empty
docs/       the exam brief (.md and .pdf)
```

## Running with Docker

`docker-compose.yml` at the repo root containerizes the **database** (SQL Server) and the **API** only — the frontend is meant to run natively via `npm run dev` against the containerized API (no container overhead needed for Vite's dev server).

```
cp .env.example .env    # first time only; sets MSSQL_SA_PASSWORD, MSSQL_DB, ports
docker compose up -d db api
```

- `db`: `mcr.microsoft.com/mssql/server:2022-latest`, exposed on `MSSQL_PORT` (default `1433`), data persisted to `database/data/` (gitignored bind mount).
- `api`: built from `backend/Dockerfile` (multi-stage `dotnet publish`), exposed on `API_PORT` (default `5232` → container port `8080`). Waits on the db's healthcheck before starting. Gets its connection string from `ConnectionStrings__DefaultConnection` (set in `docker-compose.yml`, pointing at `Server=db,1433`).
- `backend/.dockerignore` excludes `bin/`/`obj/` — without it, host-restored `obj/` folders get copied into the build context and break `dotnet publish` inside the container (this happened once; keep the `.dockerignore` in place).
- The project must also work **without** Docker (local SQL Server install + `dotnet run` / `npm run dev`); a `README.md` documenting both startup paths is still owed once the app is functionally complete — don't write it prematurely.

## Backend

.NET 10 solution at `backend/ProCredit.EmployeeManagement.slnx`, layered into four projects under `backend/src/`:

- **`ProCredit.EmployeeManagement.Domain`** — no dependencies. Entities/domain model live here.
- **`ProCredit.EmployeeManagement.Application`** — depends on Domain. Use cases / application services.
- **`ProCredit.EmployeeManagement.Infrastructure`** — depends on Application + Domain. Has `Microsoft.EntityFrameworkCore.SqlServer` + `Microsoft.EntityFrameworkCore.Design` already referenced; this is where the `DbContext`, EF configs, and migrations belong.
- **`ProCredit.EmployeeManagement.Api`** — depends on Application + Infrastructure. ASP.NET Core Web API host; `Microsoft.AspNetCore.Authentication.JwtBearer` is already referenced for the Bearer-token requirement.

All four projects currently only contain SDK-generated placeholders (`Class1.cs`, the default `WeatherForecastController`) — nothing domain-specific has been built yet.

### Commands

```
cd backend
dotnet build                                                  # build the whole solution
dotnet run --project src/ProCredit.EmployeeManagement.Api     # run the API (see launchSettings for ports)
dotnet watch --project src/ProCredit.EmployeeManagement.Api   # run with hot reload
```

- API listens on `http://localhost:5232` (`https` profile adds `https://localhost:7040`) — see `Properties/launchSettings.json`.
- No test project exists yet under `backend/`. If you add one, wire it into the `.slnx` and use `dotnet test`.
- `appsettings.Development.json` is gitignored (local secrets/connection strings go there); `appsettings.json` is the checked-in base config.

## Frontend

React 19 + TypeScript + Vite app in `frontend/`. Key deps already installed: `@mui/material` (+ `@emotion/*`) for UI components, `@tanstack/react-query` for server state, `axios` for HTTP, `react-router-dom` for routing. Linting is via `oxlint` (config: `frontend/.oxlintrc.json`), not ESLint.

`frontend/src/App.tsx` is still the default Vite/React starter page — no routes, auth, or employee screens exist yet.

### Commands

```
cd frontend
npm install
npm run dev        # start Vite dev server
npm run build       # tsc -b && vite build (type-checks before building)
npm run lint         # oxlint
npm run preview     # preview a production build
```

No test runner is configured yet.

## Database

`database/` is currently empty (aside from the gitignored `data/` bind-mount dir created by `docker compose up db`). Per the brief this needs SQL Server DDL: normalized tables for Employee/Area/Cargo with PK/identity + FK constraints, plus one stored procedure for employee queries. See [Running with Docker](#running-with-docker) above for how the DB container is provisioned.
