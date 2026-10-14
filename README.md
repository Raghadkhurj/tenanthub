# TenantHub

TenantHub is a backend API for a multi-company application.
Each company has its own users, and users can only see their own company's data.

## How to run
1. Make sure .NET SDK and SQL Server are installed
2. Open a terminal in the project folder
3. Install dotnet-ef: `dotnet tool install --global dotnet-ef`
4. Make a copy of `.env.example`, name it `.env`, then fill in:
   - The connection string (server name and database name)
   - `ADMIN_EMAIL` and `ADMIN_PASSWORD` for the first admin
   - `JWT_KEY`: a random secret of at least 32 characters
5. Build the database: `dotnet ef database update`
6. Run: `dotnet run` (the first admin and two sample companies are created automatically)
7. Open the link shown in the terminal

## How to test
- Admin login → POST /auth/admin/login with the admin email and password from `.env` Copy the returned token.
- Create user → POST /users (with admin token) with the admin token as `Authorization: Bearer <token>`.
 Body: `email`, `password`, `companySlug` (`innovation-grid` or `orange-company`).
- User login → `POST /auth/user/login`with `email`, `password`, `companySlug`. Copy the returned token.
- Current user → GET /current-user (with user token)`GET /current-user` with the user token ,returns the user and their company only.
- Login page → open `/login.html` in the browser. After a successful login, the user is redirected to a blank home page.


Automated tests will be added later and run with:
```
dotnet test
```


