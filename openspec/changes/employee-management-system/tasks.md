## 1. Database schema (EF Core migrations)

- [ ] 1.1 Add `Area`, `Cargo`, `Employee`, and `Users` entity classes in `Domain` (Employee: DocumentId, FirstName, LastName, Age, MonthlySalary, AreaId, CargoId)
- [ ] 1.2 Add EF Core entity configurations in `Infrastructure` (PK/identity columns, unique index on `Employee.DocumentId`, FK constraints Employee→Area and Employee→Cargo)
- [ ] 1.3 Add `AppDbContext` with `DbSet`s for Area, Cargo, Employee, Users
- [ ] 1.4 Create initial EF Core migration (`dotnet ef migrations add InitialCreate`)
- [ ] 1.5 Add seed data in the migration: 7 Areas (Recursos Humanos, Finanzas, Contabilidad, Marketing, Sistemas, Banca Empresas, Banca Personas), sample Cargos, and the one preconfigured test user (hashed password)
- [ ] 1.6 Add a raw-SQL migration (or `migrationBuilder.Sql(...)`) creating `sp_GetEmployeesByArea(@AreaName NVARCHAR(100) = NULL)`, joining Employee/Area/Cargo, filtering when `@AreaName` is provided
- [ ] 1.7 Apply migrations against the Dockerized SQL Server (`docker compose up -d db`, then `dotnet ef database update`) and verify tables/FKs/proc exist

## 2. Backend — Domain & Application layers

- [ ] 2.1 Define `Employee`, `Area`, `Cargo` domain entities (if not already covered by 1.1) with any invariants (e.g., age > 0)
- [ ] 2.2 Define Application DTOs: `EmployeeDto`, `CreateEmployeeRequest`, `AreaDto`, `CargoDto`, `LoginRequest`, `LoginResponse`
- [ ] 2.3 Define repository interfaces in Application: `IEmployeeRepository` (GetAll/GetByArea via stored proc, Add), `IAreaRepository`, `ICargoRepository`, `IUserRepository`
- [ ] 2.4 Implement use cases/services: `ListEmployeesService` (optional area filter), `CreateEmployeeService` (validates Area/Cargo exist, validates unique DocumentId), `AuthenticateUserService`

## 3. Backend — Infrastructure layer

- [ ] 3.1 Implement `IEmployeeRepository` using `FromSqlRaw`/`FromSqlInterpolated` against `sp_GetEmployeesByArea`, mapped to a keyless entity/DTO
- [ ] 3.2 Implement `IAreaRepository`, `ICargoRepository`, `IUserRepository` against `AppDbContext`
- [ ] 3.3 Implement password hashing/verification for the seeded test user (e.g., `PasswordHasher<T>`)
- [ ] 3.4 Register `AppDbContext` and repositories in DI (`Infrastructure` service collection extension)

## 4. Backend — API layer & auth

- [ ] 4.1 Configure JWT Bearer authentication in `Api` (signing key from config, issuer/audience, token validation parameters)
- [ ] 4.2 Add `AuthController` with `POST /api/auth/login` issuing a JWT on valid credentials, 401 on invalid
- [ ] 4.3 Add `EmployeesController` with `GET /api/employees` (optional `?area=` query param), `POST /api/employees`, both `[Authorize]`
- [ ] 4.4 Add `AreasController`/`CargosController` read-only endpoints (`GET /api/areas`, `GET /api/cargos`) for populating the add-employee modal dropdowns
- [ ] 4.5 Add request validation (data annotations or FluentValidation) for `CreateEmployeeRequest` and `LoginRequest`
- [ ] 4.6 Wire up Swagger/OpenAPI with Bearer auth support for manual testing
- [ ] 4.7 Manually verify all endpoints via `dotnet run`/Swagger against the Dockerized DB: login → token → list → filter → create → duplicate-document rejection

## 5. Frontend — App shell, routing, auth

- [ ] 5.1 Set up `react-router-dom` routes: `/login` (public), `/employees` (protected)
- [ ] 5.2 Add an axios instance with request interceptor (attach `Authorization: Bearer <token>`) and response interceptor (redirect to `/login` on 401)
- [ ] 5.3 Add auth state/context storing the JWT (in-memory + `localStorage`) with login/logout helpers
- [ ] 5.4 Add a route guard component that redirects unauthenticated users from `/employees` to `/login`

## 6. Frontend — Login screen

- [ ] 6.1 Build the login form (MUI components) with username/password fields and validation
- [ ] 6.2 Wire submit to `POST /api/auth/login`, store token on success and navigate to `/employees`
- [ ] 6.3 Show an error message on failed login without navigating away

## 7. Frontend — Employee list & filter

- [ ] 7.1 Build the employee list screen with an MUI `Table` bound to a `useQuery` call to `GET /api/employees` (fires on mount)
- [ ] 7.2 Add an Area filter control (populated from `GET /api/areas`) that re-runs the query against `GET /api/employees?area=` with the area in the `queryKey`
- [ ] 7.3 Handle loading and empty states in the table

## 8. Frontend — Add employee modal

- [ ] 8.1 Build the add-employee modal (MUI `Dialog`) with fields for document ID, first name, last name, age, monthly salary, Area select, Cargo select
- [ ] 8.2 Populate Area/Cargo selects from `GET /api/areas` and `GET /api/cargos`
- [ ] 8.3 Wire submit to a `useMutation` calling `POST /api/employees`; on success, invalidate the employee list query and close the modal
- [ ] 8.4 Surface validation/error responses (e.g., duplicate document ID) in the modal

## 9. End-to-end verification

- [ ] 9.1 Run `docker compose up -d db api` and `npm run dev`; walk through login → list loads on entry → filter by area → add employee → list refreshes
- [ ] 9.2 Confirm `dotnet build` (backend) and `npm run build` (frontend) both pass cleanly
