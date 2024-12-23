Employee Management System
A modern full-stack application for managing employees and managers, built with React and .NET Core.
🚀 Tech Stack
Backend

.NET 8.0 Web API
Entity Framework Core with SQLite
JWT Authentication
Serilog for logging
AutoMapper
Swagger/OpenAPI

Frontend

React 18
TypeScript
Tailwind CSS
React Query
React Hook Form with Zod validation
Axios

⚙️ Prerequisites

.NET 8.0 SDK
Node.js (LTS version)
Git

🛠️ Installation & Setup

Clone the repository
bashCopygit clone https://github.com/EsterBlassLev/employee-management.git
cd employee-management

Backend Setup
bashCopycd Backend
dotnet restore
dotnet ef database update
dotnet run
The API will be available at https://localhost:44356
Frontend Setup
bashCopycd ../Frontend
npm install
npm run dev
The application will be available at http://localhost:5000

🎯 Key Features

Secure JWT authentication
Real-time search with autocomplete
Comprehensive employee management (CRUD operations)
Input validation on both client and server
Responsive UI with dark/light mode
Centralized error handling and logging

🏗️ Project Structure
├── Backend/
│   ├── EmployeeManagement/
|     |-------Controllers
|     |-------DTOs
|     |-------Middleware
|     |-------Services
│   ├── EmployeeManager.Core/
|     |-----DTOs
|     |-----Exceptions
|     |-----Extensions
|     |-----Interfaces
|     |-----Models
│   ├── EmployeeManager.Infrastructure/
|     |-----Data
|     |-----Mapping
|     |-----Migrations
|     |-----Repositories
|     |-----Services
└── Frontend/
    ├── src/
    │   ├── components/
    │   ├── pages/
    │   ├── services/
    │   └── types/
    └── public/
🔑 Initial Login
you need to register and login to test the application

💡 Technical Decisions

SQLite: Chosen for its simplicity in setup and portability. The database file is included in the repository for immediate testing.
Entity Framework Core: Implements repository pattern with Code-First approach for maintainable and testable data access.
Serilog: Provides structured logging with various sinks (console, file) for better debugging and monitoring.
React Query: Offers powerful data synchronization, caching, and state management capabilities.

📝 API Documentation
After running the backend, visit https://localhost:44356/swagger for interactive API documentation.
🔐 Security Features

Password hashing using BCrypt
JWT token authentication
HTTPS enabled
CORS configuration
Input sanitization
XSS protection

📈 Performance Optimizations

Efficient SQL queries with proper indexing
React components optimization with useMemo and useCallback
API response caching
Lazy loading of routes
Minified production builds
