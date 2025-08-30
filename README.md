# Barato Product Inventory App

A full-stack Product Inventory Management Application built with ASP.NET Core, Blazor, SQL Server, and Redis.

## Features

- **Product Management**: Create, Edit, Delete, and View products
- **Product Listing**: Display products in a table with sorting and search capabilities
- **Redis Caching**: Cache product lists for improved performance
- **SQL Server Database**: Persistent data storage with Entity Framework Core
- **Docker Support**: Containerized application for easy deployment

## Tech Stack

| Layer      | Technology                    |
| :--------- | :---------------------------- |
| Backend    | ASP.NET Core Web API (.NET 8) |
| Frontend   | Blazor Server                 |
| ORM        | Entity Framework Core         |
| Database   | SQL Server (Docker)           |
| Caching    | Redis                         |
| Deployment | Docker & Docker Compose       |
| Testing    | NUnit + Moq                   |

## Project Structure

```
BaratoInventory/
├── src/
│   ├── BaratoInventory.API/          # ASP.NET Core Web API
│   ├── BaratoInventory.Blazor/       # Blazor Frontend
│   ├── BaratoInventory.Core/         # Entities, Interfaces
│   └── BaratoInventory.Infrastructure/ # EF Core, Redis, Repositories
├── tests/
│   └── BaratoInventory.Tests/        # Unit tests
├── docker-compose.yml                 # Docker Compose configuration
└── README.md
```

## Getting Started

### Prerequisites

- .NET 8 SDK
- Docker Desktop
- Git

### Running with Docker Compose

1. Clone the repository
2. Navigate to the project directory
3. Run the application:

   ```bash
   docker-compose up -d
   ```

4. Access the application:
   - Blazor UI: http://localhost:5000
   - API: http://localhost:5001
   - SQL Server: localhost,1433
   - Redis: localhost:6379

### Running Locally

1. Ensure SQL Server and Redis are running
2. Update connection strings in `appsettings.json`
3. Run the API project:
   ```bash
   cd src/BaratoInventory.API
   dotnet run
   ```
4. Run the Blazor project:
   ```bash
   cd src/BaratoInventory.Blazor
   dotnet run
   ```

## API Endpoints

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product

## Testing

Run the unit tests:

```bash
cd tests/BaratoInventory.Tests
dotnet test
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.
