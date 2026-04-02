# CoreFitness

CoreFitness is a gym portal built with ASP.NET Core MVC.

The application allows users to:

- register an account
- sign in and sign out
- create and view a membership
- view available training classes
- book a class
- cancel a booking
- view personal information on My Account
- edit profile information
- delete account and personal data

## Admin Access

For testing admin functionality, use the following credentials:

- Email: admin@corefitness.com  
- Password: Admin123!

The admin user is automatically seeded on application startup.

### What admin can do:
- Create training classes
- Delete training classes
- Access admin functionality not available to regular users

## Technologies

- ASP.NET Core MVC
- Entity Framework Core
- ASP.NET Core Identity
- SQL Server
- xUnit

## Project structure

The solution is divided into separate layers:

- Domain
- Application
- Infrastructure
- Presentation.WebApp
- Tests

## How to run the project locally

1. Clone the repository
2. Open the solution in Visual Studio 2022
3. Update the database
4. Start the web project

## Database

The project uses Entity Framework Core with Code First and migrations.

Run the following command in Package Manager Console:

```powershell
Update-Database
