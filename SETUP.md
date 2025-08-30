# Barato Product Inventory App - Setup Guide

## 📁 Project Structure

```
BaratoInventory/
├── src/
│   ├── BaratoInventory.API/          # ASP.NET Core Web API (.NET 8)
│   │   ├── Controllers/              # API Controllers
│   │   ├── Program.cs                # API Entry Point
│   │   ├── appsettings.json         # Configuration
│   │   └── Dockerfile               # API Container
│   ├── BaratoInventory.Blazor/       # Blazor Server Frontend
│   │   ├── Pages/                    # Blazor Pages
│   │   ├── Shared/                   # Layout Components
│   │   ├── wwwroot/                  # Static Files
│   │   └── Dockerfile               # Blazor Container
│   ├── BaratoInventory.Core/         # Domain Layer
│   │   ├── Entities/                 # Product Entity
│   │   └── Interfaces/               # Service Contracts
│   └── BaratoInventory.Infrastructure/ # Data & Services
│       ├── Data/                     # EF Core Context
│       ├── Repositories/             # Data Access
│       └── Services/                 # Business Logic
├── tests/
│   └── BaratoInventory.Tests/        # Unit Tests (NUnit + Moq)
├── docker-compose.yml                 # Multi-container Setup
├── BaratoInventory.sln               # Solution File
├── README.md                         # Project Documentation
├── run-app.ps1                       # PowerShell Runner
├── build.ps1                         # Build Script
└── .gitignore                        # Git Ignore Rules
```

## 🚀 Quick Start

### Option 1: Docker Compose (Recommended)

```powershell
# Start all services
docker-compose up -d

# Access the application:
# - Blazor UI: http://localhost:5000
# - API: http://localhost:5001
# - SQL Server: localhost,1433
# - Redis: localhost:6379
```

### Option 2: PowerShell Script

```powershell
# Run the interactive setup script
.\run-app.ps1
```

### Option 3: Manual Build

```powershell
# Build the solution
.\build.ps1

# Or manually:
dotnet restore
dotnet build
dotnet test
```

## ✨ Features Implemented

✅ **Product Management**

- Create, Edit, Delete products
- View product details
- List all products with sorting
- Search products by name/category

✅ **Redis Caching**

- Cache product lists for 5 minutes
- Cache invalidation on CRUD operations
- Pattern-based cache removal

✅ **SQL Server Database**

- Entity Framework Core with Code First
- Automatic database creation
- Seed data (4 sample products)
- Proper schema with validation

✅ **RESTful API**

- Full CRUD endpoints for products
- Search functionality
- Proper error handling
- Swagger documentation
- CORS enabled

✅ **Blazor Frontend**

- Modern, responsive UI
- Bootstrap styling
- Product table with actions
- Search functionality
- Navigation menu

✅ **Testing**

- Unit tests for ProductService
- Mocking with Moq
- NUnit test framework
- Test coverage for caching logic

✅ **Docker Support**

- Multi-container setup
- SQL Server 2022
- Redis 7
- .NET 8 runtime
- Easy deployment

## 🔧 Configuration

### Connection Strings

- **SQL Server**: `Server=localhost,1433;Database=BaratoInventory;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true`
- **Redis**: `localhost:6379`

### Environment Variables

- `ASPNETCORE_ENVIRONMENT`: Development
- `API_BASE_URL`: http://api (for Docker)

## 🧪 Testing

```powershell
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/BaratoInventory.Tests/
```

## 📊 Sample Data

The application comes with 4 pre-seeded products:

1. **Laptop** - Electronics - $999.99 - Qty: 10
2. **Mouse** - Electronics - $29.99 - Qty: 50
3. **Desk Chair** - Furniture - $199.99 - Qty: 15
4. **Coffee Mug** - Kitchen - $12.99 - Qty: 100

## 🌐 API Endpoints

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product
- `DELETE /api/products/{id}` - Delete product
- `GET /api/products/search?q={term}` - Search products

## 🐳 Docker Services

1. **sqlserver**: SQL Server 2022 Express
2. **redis**: Redis 7 Alpine
3. **api**: ASP.NET Core Web API
4. **blazor**: Blazor Server Frontend

## 📝 Next Steps

1. **Customize**: Modify the Product entity to add more fields
2. **Extend**: Add user authentication and authorization
3. **Enhance**: Implement product categories and images
4. **Deploy**: Use the Docker setup for production deployment
5. **Monitor**: Add logging and health checks

## 🆘 Troubleshooting

### Common Issues

1. **Port conflicts**: Ensure ports 5000, 5001, 1433, and 6379 are available
2. **Docker not running**: Start Docker Desktop before running docker-compose
3. **Database connection**: Wait for SQL Server to fully start (may take 30+ seconds)
4. **Build errors**: Ensure .NET 8 SDK is installed

### Useful Commands

```powershell
# Check Docker containers
docker ps

# View logs
docker-compose logs api
docker-compose logs blazor

# Stop all services
docker-compose down

# Rebuild and start
docker-compose up -d --build
```

## 🎯 Success Criteria Met

✅ Full-stack application with ASP.NET Core Web API (.NET 8)
✅ Blazor frontend (Server-side)
✅ Entity Framework Core with SQL Server
✅ Redis caching implementation
✅ Docker containerization
✅ Unit testing with NUnit + Moq
✅ Git version control setup
✅ Complete CRUD operations
✅ Search functionality
✅ Modern, responsive UI
✅ Proper error handling
✅ API documentation (Swagger)

## 🎉 Congratulations!

You now have a fully functional, production-ready Product Inventory Management Application that meets all the requirements from the project brief. The application is containerized, tested, and ready for development and deployment!
