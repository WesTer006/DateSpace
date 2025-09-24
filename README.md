# Datespace

A prototype dating application built with **ASP.NET Core**, **Entity Framework Core**, and **SQL Server**. The project was developed as a university course project to practice building modern web applications with authentication, recommendations, and clean architecture principles.

## Features

* User registration and login (JWT authentication)
* Profile management (interests, location, preferences)
* Photo upload and management
* Swipe-based matching system
* Recommendation service for potential matches
* Admin panel for user moderation (optional)

## Tech Stack

* **Backend:** C#, ASP.NET Core Web API
* **Database:** SQL Server, Entity Framework Core
* **Authentication:** JWT
* **Frontend (if included):** Angular / simple Razor pages (adjust depending on what you used)
* **Tools:** Git, Postman, Swagger

## Installation

1. Clone the repository:

   git clone https://github.com/WesTer006/DateSpace/tree/dev
   cd Datespace

2. Set up the database:

   * Update `appsettings.json` with your SQL Server connection string.
   * Apply migrations:

     dotnet ef database update

3. Run the application:

   dotnet run

4. Open Swagger (if configured) at:

   https://localhost:5001/swagger

## Usage

* Register a new user.
* Create a profile and upload photos.
* Swipe through profiles to find matches.
* Check matched users and start communication (if implemented).

## Project Structure

* `Domain/` – business logic, entities, value objects
* `Application/` – services, DTOs, interfaces
* `Infrastructure/` – EF Core, database context, repositories
* `API/` – controllers, endpoints

## Future Improvements

* Add chat/messaging functionality
* Enhance recommendation algorithm
* Deploy to cloud (Azure / AWS)

