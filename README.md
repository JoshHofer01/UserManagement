# User Management API

The **User Management API** is a simple RESTful API for managing user data. It allows clients to perform CRUD operations on user records, including creating, reading, updating, and deleting users. The API also includes input validation, logging, and error handling. 
This project was created for the Microsoft Back-End Developer course on Coursera, utilizing the taught content and Copilot to create this API.

## Features

- **CRUD Operations**: Create, Read, Update, and Delete users.
- **Input Validation**: Ensures user data (e.g., name and email) is valid and properly formatted.
- **Logging**: Logs incoming requests and outgoing responses using Serilog.
- **Error Handling**: Provides meaningful error responses for invalid requests or server errors.
- **Swagger Integration**: API documentation and testing via Swagger UI.

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) or later
- [Visual Studio](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- Optional: [Postman](https://www.postman.com/) for API testing

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd UserManagementAPI/UserManagementAPI
```

### 2. Build the Project

Run the following command to restore dependencies and build the project:

```bash
dotnet build
```

### 3. Run the Application

Start the application using:

```bash
dotnet run
```

The API will be available at `https://localhost:5001` (or `http://localhost:5000`).

### 4. Access Swagger UI

Navigate to `https://localhost:5001/swagger` to view the API documentation and test endpoints.

## API Endpoints

### Users

- **GET** `/api/User`: Retrieve a list of users (supports optional `limit` query parameter).
- **GET** `/api/User/{id}`: Retrieve a user by ID.
- **POST** `/api/User`: Add a new user (requires `Name` and `Email` in the request body).
- **PUT** `/api/User/{id}`: Update an existing user by ID.
- **DELETE** `/api/User/{id}`: Delete a user by ID.

## Middleware

- **Request/Response Logging**: Logs details of incoming requests and outgoing responses.
- **Global Exception Handling**: Catches unhandled exceptions and returns a standardized error response.

## Input Validation

The `InputValidationService` ensures that user input is valid:
- Names must contain only letters, spaces, or hyphens.
- Emails must follow a valid email format.

## Error Handling

The API returns structured error responses:
- **400 Bad Request**: For invalid input or missing data.
- **404 Not Found**: When a requested resource does not exist.
- **500 Internal Server Error**: For unexpected server errors.

## Logging

The API uses [Serilog](https://serilog.net/) for logging. Logs are written to the console and include details about requests, responses, and errors.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.
