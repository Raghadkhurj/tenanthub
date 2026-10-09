# TenantHub

TenantHub is a backend API for a multi-company application.
Each company has its own users, and users can only see their own company's data.

## How to run
1. Make sure .NET SDK is installed
2. Open a terminal in the project folder
3. Run: dotnet run
4. Open the link shown in the terminal

## How to test
Currently, you can test the API manually:
1. Run the project
2. Open `/weatherforecast` in the browser and check that it returns data

Automated tests will be added later and run with:
```
dotnet test
```