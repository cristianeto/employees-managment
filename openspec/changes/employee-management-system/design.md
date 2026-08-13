## Context

Greenfield build on top of three existing scaffolds (`backend/`, `frontend/`, empty `database/`) described in `CLAUDE.md`. The stack, project layout, and key dependencies are fixed by the exam brief and the scaffold already in the repo:

- SQL Server (via `docker-compose.yml`, `mcr.microsoft.com/mssql/server:2022-latest`), connected to from the API through `ConnectionStrings__DefaultConnection`.
- .NET 10 solution with four layered projects (`Domain` → `Application` → `Infrastructure` → `Api`), EF Core SQL Server + JWT Bearer packages already referenced.
- React 19 + Vite + TypeScript, with MUI, react-query, axios, and react-router-dom already installed.

Single reviewer/grader is the primary "stakeholder" — the brief emphasizes normalization, layered architecture, and a working Bearer-token flow with a preconfigured user (no self-registration, no password reset, no multi-user RBAC).

## Goals / Non-Goals

**Goals:**
- Normalized schema where Area and Cargo are independent tables; renaming either is a single-row update.
- One stored procedure that the API calls for at least the department-filter query.
- JWT Bearer auth using one hardcoded/seeded test user (username + password checked server-side, no external identity provider).
- Three API endpoints: list all employees, create employee, filter employees by area.
- Three frontend screens: login, employee list (loads on mount), add-employee modal — using MUI + react-query + axios, gated behind the JWT.
- EF Core Code-First migrations as the mechanism to create/version the schema (so `dotnet ef database update` reproducibly builds the DB the stored procedure and tables live in), with the stored procedure added via a raw-SQL migration step.

**Non-Goals:**
- Multi-user registration/management, password reset, refresh tokens, or role-based authorization beyond "authenticated or not."
- Editing/deleting employees, editing Areas/Cargos via UI (seed them directly in the DB/migration).
- Pagination, sorting, or advanced filtering beyond area/department.
- Production-grade secrets management (JWT signing key can live in `appsettings.Development.json`, gitignored).
- Automated test suite (no test project exists yet per `CLAUDE.md`; out of scope unless requested later).

## Decisions

**Schema shape**: `Area(Id, Name)`, `Cargo(Id, Name)`, `Employee(Id, DocumentId unique, FirstName, LastName, Age, MonthlySalary, AreaId FK, CargoId FK)`. `Id` columns are `IDENTITY(1,1)` PKs; `DocumentId` gets a unique constraint (business key from the brief) but the surface PK stays a synthetic identity, which is simpler for EF Core FK plumbing and avoids exposing document numbers in URLs.
- *Alternative considered*: `DocumentId` as PK. Rejected — string/natural PKs complicate FK indexing and EF conventions for marginal benefit here.

**Stored procedure**: `sp_GetEmployeesByArea(@AreaName NVARCHAR(100) = NULL)` — returns all employees (joined with Area/Cargo names) when `@AreaName` is NULL, or filtered when provided. This single proc satisfies both "list all" and "filter by department" reads, matching the brief's "1 stored procedure for employee queries" requirement while giving the API a real SP to call instead of writing a proc no endpoint uses.
- *Alternative considered*: separate proc for list-all and a LINQ-only filter. Rejected — brief asks for exactly one proc; better to make it earn its place by backing the more interesting (filtered) query, and reuse it for list-all via NULL parameter.

**EF Core calling a stored procedure**: use `FromSqlRaw`/`FromSqlInterpolated` against a keyless entity/DTO mapped with `.HasNoKey()`, rather than `ExecuteSqlRaw` + manual `SqlCommand`. Keeps it inside the `DbContext`/repository abstraction already scaffolded in `Infrastructure`.

**Auth**: a single seeded user (username/password hash) stored in a small `Users` table (or `appsettings` — DB table preferred so it goes through the same EF Core/migration pipeline as everything else). `POST /api/auth/login` validates credentials and issues a JWT (HMAC-SHA256, symmetric key from config) with a short-ish expiry (e.g., 60 min). All employee endpoints require `[Authorize]`.
- *Alternative considered*: hardcoded credentials check in code with no Users table. Rejected in favor of a Users table — trivial extra effort, keeps auth data-driven and consistent with "everything is normalized in the DB."

**API surface**:
- `POST /api/auth/login` — body `{ username, password }` → `{ token }`.
- `GET /api/employees` — list all (calls the SP with NULL).
- `GET /api/employees?area={areaName}` — filter by area (calls the SP with the parameter); reuses the same controller action as list-all via an optional query param rather than a separate route, since both are backed by the same SP and DTO shape.
- `POST /api/employees` — create; body includes `documentId, firstName, lastName, age, monthlySalary, areaId, cargoId`.
- Areas/Cargos exposed read-only (`GET /api/areas`, `GET /api/cargos`) only as needed to populate the add-employee modal's dropdowns — not called out explicitly in the brief but required for the modal to be usable without hardcoding IDs client-side.

**Frontend architecture**: `react-router-dom` with two routes — `/login` (public) and `/employees` (protected, redirects to `/login` if no token). JWT stored in memory + `localStorage` (simplest viable option for a take-home; no refresh-token rotation needed). Axios instance with a request interceptor attaching `Authorization: Bearer <token>`, and a response interceptor redirecting to `/login` on 401. `react-query` `useQuery` for list/filter (`queryKey` includes the area filter so switching filters is cached separately), `useMutation` for create, invalidating the list query on success. MUI `Dialog` for the add-employee modal, MUI `Table`/`DataGrid`-equivalent for the list (`Table` from `@mui/material` is sufficient — no need for the paid `DataGrid Pro`).

## Risks / Trade-offs

- **[Risk]** Storing JWT in `localStorage` is XSS-exposed → **Mitigation**: acceptable for a take-home exam scope (no untrusted third-party scripts); note as a known trade-off, not a production pattern.
- **[Risk]** Single seeded user with no registration flow could look "incomplete" to a grader expecting user management → **Mitigation**: brief explicitly asks for "un usuario de prueba preconfigurado," so this matches spec; call it out clearly in the eventual README.
- **[Risk]** Keyless entity mapping for the SP result adds EF Core configuration complexity → **Mitigation**: fallback is a manual `SqlCommand`/`Dapper`-style read if `FromSqlRaw` + keyless entity proves awkward; keep the repository interface stable so the implementation can swap without touching Application/Api layers.
- **[Risk]** Seeding Area/Cargo reference data only via migration means changing them later requires a new migration → **Mitigation**: acceptable since the brief doesn't ask for Area/Cargo CRUD UI; document as a deliberate scope cut.

## Migration Plan

1. DB: EF Core migration creates `Area`, `Cargo`, `Employee`, `Users` tables + FKs + unique index on `DocumentId`, seeds the 7 areas, sample cargos, and the one test user; a follow-up raw-SQL migration (or `Migrator.Sql` step) creates `sp_GetEmployeesByArea`.
2. Backend: implement bottom-up — Domain entities → Application DTOs/interfaces → Infrastructure (`DbContext`, configs, repo, migrations) → Api (controllers, JWT middleware, DI).
3. Frontend: routing/auth shell → login page → employee list page wired to `GET /api/employees` → add-employee modal wired to `POST /api/employees` and the area filter wired to the same list query.
4. No rollback complexity beyond `dotnet ef database update <previous-migration>` — this is a fresh schema, not a change to production data.

## Open Questions

- Exact test-user credentials (e.g., `admin`/`Password123!`) — will seed something obvious and document it in the eventual README; no functional impact either way.
- Whether the grader expects the department filter as a query param on `GET /api/employees` vs. a distinct `GET /api/employees/search` route — current design picks the query-param approach for simplicity; easy to add a distinct route later if needed.
