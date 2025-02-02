# User Management System Project

This is a User Management System built using .NET 6 and Angular 17. It follows the Clean Architecture pattern and implements CQRS (Command Query Responsibility Segregation) for handling user registration, login, and management. The system is designed for scalability and security with a focus on high performance and maintainability.

## Technologies Used

- **Backend:** 
  - ASP.NET Core (.NET 6)
  - CQRS (Command Query Responsibility Segregation)
  - Clean Architecture

- **Frontend:**
  - Angular 17
  - Standalone Components

- **Other Technologies:**
  - AWS S3 (for image storage)
  - Syncfusion (for UI components)
  - MongoDB (for data storage)
  - Hangfire (for background job processing)
  - Twilio, Mailchimp, SendGrid (for integrations)

## Project Structure

The solution consists of multiple projects, each serving a distinct purpose:

1. **src**: Contains the core project files, including both the backend and frontend components.
2. **tests**: Unit and integration tests for the entire solution.
3. **UserManagementSystem.API**: The Web API project responsible for handling HTTP requests.
4. **UserManagementSystem.Application**: The application layer implementing business logic.
5. **UserManagementSystem.Domain**: The domain layer that defines core business entities and interfaces.
6. **UserManagementSystem.Infrastructure**: The infrastructure layer that implements data access, external services, and other system-related functionalities.
7. **UserManagementSystem.Application.UnitTest**: Unit tests for the application layer.

### Nested Projects:
- All individual projects are nested under the `src` folder for easy navigation.

## Setup Instructions

Follow the steps below to set up and run the project on your local machine.

### Prerequisites

- **Visual Studio 2022 (Version 17.x or higher)**
- **.NET 6 SDK**
- **Node.js** (for Angular front-end development)
- **MongoDB** (for local development, if you prefer local storage over cloud solutions)
- **AWS Account** (for integrating AWS services)
