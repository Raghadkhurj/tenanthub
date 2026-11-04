# TenantHub

TenantHub is a backend API for a multi-company application.
Each company has its own users, and users can only see their own company's data.
The frontend is in a separate project: `tenanthub-web` (Angular).

## How to run
1. Make sure .NET SDK and SQL Server are installed
2. Open a terminal in the project folder
3. Install dotnet-ef: `dotnet tool install --global dotnet-ef`
4. Make a copy of `.env.example`, name it `.env`, then fill in:
   - The connection string (server name and database name)
   - `ADMIN_EMAIL` and `ADMIN_PASSWORD` for the first admin
   - `JWT_KEY`: a random secret of at least 32 characters
   - `FRONTEND_ORIGIN`: the Angular app URL (`http://localhost:4200`), the only origin allowed by CORS
5. Build the database: `dotnet ef database update`
6. Run: `dotnet run` (the first admin and two sample companies are created automatically)

## How to test
- Admin login → `POST /auth/admin/login` with the admin email and password from `.env`. Copy the returned token.
- Create user → `POST /users` with the admin token as `Authorization: Bearer <token>`.
  Body: `email`, `password`, `companySlug` (`innovation-grid` or `orange-company`).
- User login → `POST /auth/user/login` with `email`, `password`, `companySlug`. Copy the returned token.
- Current user → `GET /current-user` with the user token. Returns the user and their company only.

Run the automated tests:
```
dotnet test TenantHub.Tests
```