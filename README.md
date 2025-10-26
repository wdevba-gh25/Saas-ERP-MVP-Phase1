# SaaS ERP MVP – Phase 1
Early-phase demo of a multi-tenant SaaS ERP built with **.NET 8** (backend) and **React 18 + Vite + TypeScript** (frontend).  
Includes authentication, organization-level isolation, GraphQL integration, and modular microservices.

---

## Architecture Overview
- **Backend:** .NET 8 microservice solution  
  - `AuthService`, `ProjectService`, `GraphQLApi`, `TenantMiddleware`, `Shared`  
  - Multi-tenant enforcement via `organizationId` claims  
  - EF Core Code-First with SQLite (dev) / SQL Server (prod)  
- **Frontend:** React 18 + Vite + TypeScript  
  - Zustand global store, Axios interceptors, Tailwind CSS  
  - Auth-protected routes and dynamic API base URL

---

## Prerequisites
The following tools must be installed **before running the project locally**:

### Backend Environment
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) with *.NET 8 SDK (8.0.x)*  
- [SQL Server Developer Edition](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or SQL Express  
- [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)  
- [Azure Data Studio (optional)] – for lightweight DB management  
- [Git](https://git-scm.com/downloads) for cloning and version control

### Frontend Environment
- [Node.js 18+ LTS](https://nodejs.org/en/download/) and npm (v9 or later)  
- [Vite CLI](https://vitejs.dev/guide/) (optional)  
- [Visual Studio Code](https://code.visualstudio.com/) with extensions:
  - Prettier – Code Formatter  
  - ESLint  
  - Tailwind CSS IntelliSense

---

## Database Setup
A ready-to-import **BACPAC** file is included in the repository:

1. Launch **SQL Server Management Studio (SSMS)**.  
2. Right-click **Databases → Import Data-tier Application…**  
3. Choose the provided `.bacpac` file (located at `/database/SaaS_ERPMVP.bacpac`).  
4. Deploy to your local SQL Server instance.  
5. Note the connection string for later use.

---

## Backend Setup

### 1 Configure connection strings
Edit `appsettings.json` in each service folder (`AuthService`, `ProjectService`, `GraphQLApi`) and replace:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=SaaSMvpDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 2 Run services
You can launch services individually or from the Visual Studio Solution:
```bash
cd backend/AuthService
dotnet run

cd ../ProjectService
dotnet run

cd ../GraphQLApi
dotnet run
```
All services register themselves on `http://localhost:<port>` (see launchSettings.json for details).

---

## Frontend Setup
```bash
cd frontend/web
npm install
npm run dev
```
- React app runs by default on `http://localhost:5173`  
- Configure API base URL in `.env` file:
  ```
  VITE_API_BASE=http://localhost:5000
  ```

---

## Authentication Workflow
- Register/Login via `AuthService /api/auth/register` and `/api/auth/login`  
- JWT includes `organizationId` claim → used by ProjectService for tenant filtering  
- Axios interceptor automatically adds `Authorization: Bearer <token>`

---

## Core Modules
| Service | Responsibility |
|:--|:--|
| **AuthService** | User registration & JWT generation |
| **ProjectService** | Project CRUD within organization |
| **GraphQLApi** | HotChocolate schema for cross-service querying |
| **TenantMiddleware** | Per-tenant context resolution |
| **Shared** | Entities, DbContext, common models |

---

## Testing & Development
- Swagger UI available for each service in development (`https://localhost:<port>/swagger`)  
- Recommended test accounts stored in seed data (if enabled in startup).  
- EF Core migration auto-executes on startup for SQLite mode.

---

## Next Steps / Future Work
- Introduce CQRS and event-driven flows via RabbitMQ or Kafka  
- Add policy-based authorization (e.g. `SameTenantRequirement`)  
- Containerize with Docker Compose (optional)  
- Extend GraphQL mutations and frontend UI modules
