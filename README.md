TaskManager API (Backend)

TaskManager is a RESTful API built with ASP.NET Core for managing projects, tasks, and user authentication. It provides endpoints for user registration, login, profile management, project and task CRUD operations, with JWT-based authentication.

Features

User Management: Register, Login, Update Profile, Delete Account.
Projects: Create, Read, Update, Delete projects.
Tasks: Create, Read, Update, Delete tasks under specific projects.
Status Management: Task statuses such as Pending, InProgress, Done, Cancelled.
JWT Authentication: Secure API access with token-based authentication.
Swagger UI: API documentation for easy testing.

Tech Stack
Backend: ASP.NET Core 8.0
Database: Entity Framework Core with SQL Server
Authentication: JWT Bearer Tokens

API Documentation: Swagger / OpenAPI

Getting Started
Prerequisites

.NET 8 SDK: Download

SQL Server or SQL Server Express

Postman or Swagger UI for testing

Installation
Clone the repository:
git clone https://github.com/yourusername/TaskManagerBackend.git
cd TaskManagerBackend
Restore packages: dotnet restore
Update the appsettings.json connection string:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=TaskManagerDB;Trusted_Connection=True;"
}
Apply migrations: dotnet ef database update

Run the API: dotnet run

By default, Swagger will be available at:
https://localhost:5001/swagger/index.html
API Endpoints
Authentication
Endpoint	Method	Description
/api/Auth/register	POST	Register new user
/api/Auth/login	POST	Login user and get JWT
/api/Auth/profile	GET	Get current user profile
/api/Auth/profile	PUT	Update user profile
/api/Auth/profile	DELETE	Delete user account
Projects
Endpoint	Method	Description
/api/Project/GetAllProjects	GET	Get all projects
/api/Project/CreateProject	POST	Create new project
/api/Project/UpdateProject/{id}	PUT	Update project by ID
/api/Project/DeleteProject/{id}	DELETE	Delete project by ID
Tasks
Endpoint	Method	Description
/api/projects/{projectId}/tasks	GET	Get tasks for a project
/api/projects/{projectId}/tasks	POST	Create a new task
/api/projects/{projectId}/tasks/{taskId}	PUT	Update task by ID
/api/projects/{projectId}/tasks/{taskId}	DELETE	Delete task by ID

Note: {projectId} and {taskId} are integers representing the IDs of projects and tasks.

Authentication

Users must authenticate via JWT token for most endpoints.

The token is obtained from /api/Auth/login.

Include the token in the Authorization header:

Authorization: Bearer YOUR_JWT_TOKEN
Database

Database is managed via Entity Framework Core migrations.

Default tables:

Users

Projects

Tasks

Relationships:

Project 1:N Tasks

User 1:N Projects (optional if you extend ownership)

Environment Variables

Recommended environment variables:

ASPNETCORE_ENVIRONMENT=Development
ConnectionStrings__DefaultConnection=Your SQL Server connection string
JWT__Key=your_super_secret_key
JWT__Issuer=your_issuer
JWT__Audience=your_audience
Running Tests
dotnet test

All tests are located in the TaskManagerBackend.Tests project.

Deployment

Build project:

dotnet publish -c Release -o ./publish

Deploy to Azure App Service or your preferred hosting provider.

Update appsettings.Production.json with production database connection.

Contributing

Fork the repository.

Create a feature branch (git checkout -b feature/YourFeature).

Commit your changes (git commit -am 'Add new feature').

Push to the branch (git push origin feature/YourFeature).

Open a Pull Request.

License

This project is licensed under the MIT License.
