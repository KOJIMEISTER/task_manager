# Task Management System API

![License](https://img.shields.io/badge/license-MIT-blue.svg)
![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16.0-blue.svg)

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
  - [Configuration](#configuration)
  - [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [Database](#database)
- [Docker Setup](#docker-setup)
- [Contributing](#contributing)
- [License](#license)

## Overview

The **Task Management System API** is a robust web application designed to streamline project and task management within organizations. Built with C#, ASP.NET Core 8, Entity Framework Core, and PostgreSQL, it offers a secure and scalable solution for managing users, projects, tasks, roles, and permissions. The API incorporates JWT-based authentication, role-based access control, and comprehensive CRUD operations to facilitate seamless collaboration and productivity.

## Features

- **User Authentication & Authorization**
  - **JWT-Based Authentication:** Secure login and access control using JSON Web Tokens.
  - **Role-Based Access Control:** Define and assign roles with specific permissions to users.
  - **Refresh Token Mechanism:** Securely refresh access tokens without re-authenticating.

- **User Management**
  - **CRUD Operations:** Create, read, update, and delete user profiles.
  - **Role Assignment:** Assign and manage user roles within the system.

- **Project Management**
  - **Project Lifecycle Management:** Create, view, update, and delete projects.
  - **User Assignment:** Assign users to projects with specific roles.

- **Task Management**
  - **Task CRUD:** Create, read, update, and delete tasks within projects.
  - **Task Assignment:** Assign tasks to specific users.
  - **Status & Priority Management:** Define and update task status and priority levels.
  - **Comments:** Add and manage comments on tasks for better collaboration.

- **File Management**
  - **Upload & Management:** Upload files related to projects and tasks.
  - **Association:** Associate files with specific projects or tasks.

- **Notifications**
  - **Real-Time Notifications:** Notify users about important updates and changes.
  - **Read Status:** Track whether notifications have been read.

- **Error Handling**
  - **Global Exception Handling:** Middleware to handle exceptions gracefully and provide meaningful responses.

## Technologies Used

- **Backend:**
  - [C#](https://docs.microsoft.com/en-us/dotnet/csharp/)
  - [ASP.NET Core 8](https://docs.microsoft.com/en-us/aspnet/core/)
  - [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
  - [PostgreSQL](https://www.postgresql.org/)

- **Authentication & Security:**
  - [JWT (JSON Web Tokens)](https://jwt.io/)
  - [Konscious.Security.Cryptography](https://github.com/kmarusha/Konscious.Security.Cryptography) for password hashing

- **Others:**
  - [AutoMapper](https://automapper.org/) for object-object mapping
  - [Docker](https://www.docker.com/) for containerization
  - [Swagger](https://swagger.io/) for API documentation

## Getting Started

### Prerequisites

- **.NET 8 SDK:** [Download .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Docker:** [Install Docker](https://www.docker.com/get-started)
- **Git:** [Install Git](https://git-scm.com/downloads)

### Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/KOJIMEISTER/task_manager.git
   cd task_manager
   ```

2. **Set Up the PostgreSQL Database**

   You can use Docker to set up a PostgreSQL instance:

   ```bash
   docker-compose up -d
   ```

   This command does the following:

   - Pulls the PostgreSQL 16 image.
   - Creates a container named `postgres-container`.
   - Sets up environment variables for the database `tmapi_db` with user `tmapi_root` and password `546258`.
   - Maps port `5432` of the container to the host.
   - Initializes the database using the `init_db.sql` script.

   *Alternatively,* if you prefer to use a local PostgreSQL installation, ensure you create a database named `tmapi_db` and update the connection strings accordingly in the configuration files.

3. **Configure Application Settings**

   The application uses `appsettings.json` and `appsettings.Development.json` for configuration.

   - **Database Connection:** Ensure the `DefaultConnection` string points to your PostgreSQL instance.
   - **JWT Settings:** Configure the `JwtSettings` section with a secure secret key.

   **Important:** Do not expose sensitive information such as database passwords or JWT secrets in public repositories. Consider using [User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets) or environment variables for sensitive configurations.

4. **Restore Dependencies**

   Navigate to the project directory and restore NuGet packages:

   ```bash
   dotnet restore
   ```

5. **Build the Project**

   ```bash
   dotnet build
   ```

### Configuration

- **Environment Variables:**

  The application supports configuration via environment variables. You can set the following variables as needed:

  - `ConnectionStrings__DefaultConnection`
  - `JwtSettings__Secret`
  - `JwtSettings__Issuer`
  - `JwtSettings__Audience`
  - `JwtSettings__ExpiryInMinuts`
  - `JwtSettings__RefreshTokenExpiryDays`

- **ASP.NET Core Environment:**

  The application defaults to the `Development` environment. You can change this by setting the `ASPNETCORE_ENVIRONMENT` variable.

### Running the Application

Start the application using the following command:

```bash
dotnet run
```

The API will be accessible at:

- **HTTP:** `http://localhost:5004`
- **HTTPS:** `https://localhost:7207`

### Running with Docker

If you prefer to run the application using Docker, create a `Dockerfile` and configure it accordingly. Ensure that it connects to the PostgreSQL instance set up via `docker-compose`.

## API Documentation

Swagger is integrated for interactive API documentation. After running the application, navigate to `/swagger` to access the Swagger UI.

Example: `https://localhost:7207/swagger`

### Authentication Endpoints

- **Login**

  ```http
  POST /api/v1/auth/login
  ```

  **Request Body:**

  ```json
  {
    "username": "string",
    "password": "string"
  }
  ```

  **Response:**

  ```json
  {
    "username": "string",
    "token": "jwt-token",
    "refreshToken": "refresh-token"
  }
  ```

- **Register**

  ```http
  POST /api/v1/auth/register
  ```

  **Request Body:**

  ```json
  {
    "username": "string",
    "password": "string",
    "email": "user@example.com"
  }
  ```

  **Response:**

  ```json
  {
    "username": "string",
    "token": "jwt-token",
    "refreshToken": "refresh-token"
  }
  ```

- **Refresh Token**

  ```http
  POST /api/v1/auth/refresh-token
  ```

  **Request Body:**

  ```json
  {
    "refreshToken": "string"
  }
  ```

  **Response:**

  ```json
  {
    "token": "new-jwt-token",
    "refreshToken": "new-refresh-token"
  }
  ```

- **Logout**

  ```http
  POST /api/v1/auth/logout
  ```

  **Headers:**

  - `Authorization: Bearer {token}`

  **Response:**

  ```json
  {
    "Message": "Logged out successfully."
  }
  ```

### User Endpoints

- **Get All Users**

  ```http
  GET /api/v1/users?pageNumber=1&pageSize=10
  ```

  **Headers:**

  - `Authorization: Bearer {token}`

  **Response:**

  ```json
  [
    {
      "id": "guid",
      "username": "string",
      "email": "user@example.com",
      "firstname": "string",
      "lastname": "string",
      "isactive": true,
      "createdat": "2023-10-01T00:00:00Z",
      "updatedat": "2023-10-01T00:00:00Z",
      "userroles": []
    }
  ]
  ```

- **Get User By ID**

  ```http
  GET /api/v1/users/{id}
  ```

  **Headers:**

  - `Authorization: Bearer {token}`

  **Response:**

  ```json
  {
    "id": "guid",
    "username": "string",
    "email": "user@example.com",
    "firstname": "string",
    "lastname": "string",
    "isactive": true,
    "createdat": "2023-10-01T00:00:00Z",
    "updatedat": "2023-10-01T00:00:00Z",
    "userroles": []
  }
  ```

- **Create User**

  ```http
  POST /api/v1/users
  ```

  **Headers:**

  - `Authorization: Bearer {admin-token}`

  **Request Body:**

  ```json
  {
    "username": "string",
    "email": "user@example.com",
    "password": "string",
    "firstname": "string",
    "lastname": "string"
  }
  ```

  **Response:**

  ```json
  {
    "id": "guid",
    "username": "string",
    "email": "user@example.com",
    "firstname": "string",
    "lastname": "string",
    "isactive": true,
    "createdat": "2023-10-01T00:00:00Z",
    "updatedat": "2023-10-01T00:00:00Z",
    "userroles": []
  }
  ```

- **Update User**

  ```http
  PUT /api/v1/users/{id}
  ```

  **Headers:**

  - `Authorization: Bearer {admin-token}`

  **Request Body:**

  ```json
  {
    "username": "string",
    "email": "user@example.com",
    "password": "string",
    "firstname": "string",
    "lastname": "string",
    "isactive": true
  }
  ```

  **Response:**

  - `204 No Content`

- **Delete User**

  ```http
  DELETE /api/v1/users/{id}
  ```

  **Headers:**

  - `Authorization: Bearer {admin-token}`

  **Response:**

  - `204 No Content`

- **Change Password**

  ```http
  PUT /api/v1/users/change-password
  ```

  **Headers:**

  - `Authorization: Bearer {token}`

  **Request Body:**

  ```json
  {
    "currentPassword": "string",
    "newPassword": "string"
  }
  ```

  **Response:**

  - `204 No Content`

### Project Endpoints

*Note: The `ProjectController` is currently partially implemented with plans for the following endpoints.*

- **Get All Projects**

  ```http
  GET /api/v1/project
  ```

  **Headers:**

  - `Authorization: Bearer {token}`

  **Response:**

  ```json
  [
    {
      "id": "guid",
      "name": "Project Name",
      "description": "Project Description",
      "startdate": "2023-10-01",
      "enddate": "2023-12-31",
      "createdby": "user-guid",
      "createdat": "2023-10-01T00:00:00Z",
      "updatedat": "2023-10-01T00:00:00Z",
      "files": [],
      "projectusers": [],
      "tasks": []
    }
  ]
  ```

- **Additional Endpoints Planned:**
  - `POST /api/v1/project` - Create a new project
  - `GET /api/v1/project/{id}` - Retrieve project details
  - `PUT /api/v1/project/{id}` - Update project details
  - `DELETE /api/v1/project/{id}` - Delete a project
  - `POST /api/v1/project/{id}/assign` - Assign users to a project
  - `GET /api/v1/project/{id}/users` - List users assigned to a project

## Database

The application uses PostgreSQL as its relational database. The database schema is defined using Entity Framework Core with code-first migrations. Additionally, an SQL script `init_db.sql` is provided to initialize the database schema and indexes.

### Database Schema Overview

- **Users:** Manage user information and authentication details.
- **Roles:** Define various roles within the system.
- **Permissions:** Specify permissions that can be assigned to roles.
- **UserRoles:** Many-to-Many relationship between Users and Roles.
- **RolePermissions:** Many-to-Many relationship between Roles and Permissions.
- **Projects:** Manage project details.
- **ProjectUsers:** Many-to-Many relationship between Projects and Users with roles.
- **Tasks:** Manage tasks within projects.
- **TaskComments:** Add comments to tasks.
- **Files:** Upload and manage files related to projects and tasks.
- **Notifications:** Send and manage user notifications.
- **RefreshTokens:** Handle JWT refresh tokens for users.

For detailed schema initialization, refer to the `init_db.sql` file in the project root.

## Docker Setup

The project includes a `docker-compose.yml` file to facilitate easy setup of the PostgreSQL database.

### Running PostgreSQL with Docker

1. **Ensure Docker is Installed and Running**

2. **Start the PostgreSQL Container**

   ```bash
   docker-compose up -d
   ```

   This command will:

   - Pull the PostgreSQL 16 image.
   - Create and start a container named `postgres-container`.
   - Set up environment variables for the database user, password, and name.
   - Map port `5432` of the container to the host.
   - Mount the `init_db.sql` script to initialize the database.
   - Use Docker volumes for persistent data storage.

3. **Verify the Container is Running**

   ```bash
   docker ps
   ```

   You should see `postgres-container` in the list of running containers.

### Connecting to the PostgreSQL Database

Use your preferred PostgreSQL client with the following credentials:

- **Host:** `localhost`
- **Port:** `5432`
- **Database:** `tmapi_db`
- **Username:** `tmapi_root`
- **Password:** `546258`

*Remember to secure your database credentials and avoid using default passwords in production environments.*

## Contributing

Contributions are welcome! To contribute:

1. **Fork the Repository**

2. **Create a Branch**

   ```bash
   git checkout -b feature/YourFeature
   ```

3. **Commit Your Changes**

   ```bash
   git commit -m "Add your feature"
   ```

4. **Push to the Branch**

   ```bash
   git push origin feature/YourFeature
   ```

5. **Open a Pull Request**

   Provide a detailed description of your changes and the problem they address.

## License

This project is licensed under the [MIT License](LICENSE).

---

## Acknowledgements

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [JWT.io](https://jwt.io/)
- [Docker](https://www.docker.com/)
- [Swagger](https://swagger.io/)

---

**Note:** This project is a work in progress. Some features and endpoints are under development and will be updated in future releases. Contributions and feedback are highly appreciated!
