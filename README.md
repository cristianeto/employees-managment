# ProCredit – Employee Management System

Sistema de gestión de empleados: base de datos SQL Server, API REST en .NET 10 (arquitectura por capas) y SPA en React 19. Proyecto desarrollado para la prueba técnica de ProCreditGroup (ver `docs/`).

## Stack

- **Base de datos**: SQL Server 2022, esquema normalizado (Employees / Areas / Cargos / Users) + stored procedure `sp_GetEmployeesByArea`, gestionado con EF Core Migrations.
- **API**: .NET 10, ASP.NET Core Web API, autenticación JWT Bearer, arquitectura en 4 capas (`Domain` → `Application` → `Infrastructure` → `Api`).
- **Frontend**: React 19 + TypeScript + Vite, MUI, React Query, React Router, Axios.

## Estructura del repo

```
backend/    Solución .NET 10 (backend/ProCredit.EmployeeManagement.slnx)
frontend/   SPA React + Vite + TypeScript
database/   Datos persistidos del contenedor SQL Server (bind mount, ignorado por git)
docs/       Enunciado de la prueba
```

## Prerrequisitos

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/) y npm
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (recomendado) **o** una instancia local de SQL Server
- Herramienta `dotnet-ef` para aplicar migraciones:
  ```
  dotnet tool install --global dotnet-ef
  ```

---

## Opción 1: Ejecutar con Docker (recomendado)

Docker se usa solo para la **base de datos** y la **API**. El frontend corre nativo con `npm run dev` (no necesita contenedor).

### 1. Configurar variables de entorno

```
cp .env.example .env
```

Valores por defecto en `.env.example` (ajustar si es necesario):

| Variable            | Default                       | Descripción                          |
|---------------------|--------------------------------|---------------------------------------|
| `MSSQL_SA_PASSWORD` | `YourStrong!Passw0rd`          | Password del usuario `sa` de SQL Server |
| `MSSQL_DB`          | `ProCreditEmployeeManagement`  | Nombre de la base de datos            |
| `MSSQL_PORT`        | `1433`                         | Puerto expuesto en el host            |
| `API_PORT`          | `5232`                         | Puerto expuesto en el host para la API |

### 2. Levantar la base de datos y la API

```
docker compose up -d db api
```

Espera a que el healthcheck de `db` pase antes de que `api` arranque (docker compose ya maneja la dependencia automáticamente).

### 3. Aplicar las migraciones de EF Core

El contenedor de la API **no** aplica migraciones automáticamente al iniciar, así que hay que ejecutarlas una vez desde el host, apuntando al SQL Server publicado por Docker (`localhost:<MSSQL_PORT>`).

Crea `backend/src/ProCredit.EmployeeManagement.Api/appsettings.Development.json` (está en `.gitignore`, no viene en el repo) con la misma contraseña que pusiste en `.env`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ProCreditEmployeeManagement;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True"
  }
}
```

Luego aplica las migraciones:

```
cd backend
dotnet ef database update --project src/ProCredit.EmployeeManagement.Infrastructure --startup-project src/ProCredit.EmployeeManagement.Api
```

Esto crea las tablas, el stored procedure `sp_GetEmployeesByArea`, y siembra los catálogos de Áreas/Cargos y el usuario de prueba.

> Si prefieres reconstruir la API después de este paso (por ejemplo, tras cambiar código), vuelve a levantarla con `docker compose up -d --build api`.

### 4. Levantar el frontend

```
cd frontend
npm install
npm run dev
```

Abre `http://localhost:5173`. El cliente HTTP apunta a `http://localhost:5232` por defecto (coincide con `API_PORT`)

---

## Opción 2: Ejecutar todo en local (sin Docker)

### 1. Base de datos

Instala/usa una instancia local de SQL Server y crea una base de datos vacía (por ejemplo `ProCreditEmployeeManagement`). No es necesario crear tablas a mano: las migraciones de EF Core las generan.

### 2. Configurar la cadena de conexión

Crea `backend/src/ProCredit.EmployeeManagement.Api/appsettings.Development.json` (gitignored):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=ProCreditEmployeeManagement;User Id=sa;Password=<tu-password>;TrustServerCertificate=True"
  }
}
```

Ajusta `Server`/credenciales según tu instalación (Windows Auth, instancia nombrada, etc.).

### 3. Aplicar migraciones

```
cd backend
dotnet ef database update --project src/ProCredit.EmployeeManagement.Infrastructure --startup-project src/ProCredit.EmployeeManagement.Api
```

### 4. Levantar la API

```
dotnet run --project src/ProCredit.EmployeeManagement.Api
```

La API queda disponible en `http://localhost:5232` (perfil `https` añade `https://localhost:7040`). Documentación OpenAPI/Scalar disponible en `/scalar` en entorno Development.

### 5. Levantar el frontend

```
cd frontend
npm install
npm run dev
```

Abre `http://localhost:5173`.

---

## Usuario de prueba

La migración de seed crea un único usuario para autenticarse:

| Usuario | Password    |
|---------|-------------|
| `admin` | `Admin123!` |

## Endpoints principales de la API

Todos (excepto login) requieren header `Authorization: Bearer <token>`.

| Método | Ruta            | Descripción                                   |
|--------|-----------------|------------------------------------------------|
| POST   | `/api/auth/login` | Autentica y devuelve el JWT                  |
| GET    | `/api/employees`  | Lista empleados; filtro opcional `?area=`    |
| POST   | `/api/employees`  | Crea un empleado                              |
| GET    | `/api/areas`       | Lista de áreas                               |
| GET    | `/api/cargos`      | Lista de cargos                              |

## Comandos útiles

**Backend** (desde `backend/`):
```
dotnet build                                                 # compila la solución
dotnet run --project src/ProCredit.EmployeeManagement.Api    # corre la API
dotnet watch --project src/ProCredit.EmployeeManagement.Api  # hot reload
```

**Frontend** (desde `frontend/`):
```
npm run dev       # servidor de desarrollo Vite
npm run build     # type-check + build de producción
npm run lint      # oxlint
npm run preview   # preview del build de producción
```

## Notas / troubleshooting

- `appsettings.Development.json` y `.env` están en `.gitignore` intencionalmente (contienen secretos locales); cada desarrollador debe crearlos siguiendo las plantillas de este README.
- Si el frontend no puede llamar a la API por CORS, revisa `Cors:AllowedOrigins` en `backend/src/ProCredit.EmployeeManagement.Api/appsettings.json` — por defecto solo permite `http://localhost:5173`.
- Si reconstruyes la imagen de la API tras cambios de código, usa `docker compose up -d --build api`.
- Los datos de SQL Server en Docker se persisten en `database/data/` (bind mount, ignorado por git). Para reiniciar la base desde cero, detén los contenedores y borra ese directorio.
