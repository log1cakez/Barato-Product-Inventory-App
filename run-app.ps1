# Barato Product Inventory App - Local Development Script
Write-Host "Starting Barato Product Inventory App..." -ForegroundColor Green

# Check if .NET 8 is installed
try {
    $dotnetVersion = dotnet --version
    Write-Host ".NET Version: $dotnetVersion" -ForegroundColor Yellow
} catch {
    Write-Host "Error: .NET 8 SDK is not installed or not in PATH" -ForegroundColor Red
    Write-Host "Please install .NET 8 SDK from: https://dotnet.microsoft.com/download/dotnet/8.0" -ForegroundColor Red
    exit 1
}

# Check if Docker is running
try {
    docker version | Out-Null
    Write-Host "Docker is running" -ForegroundColor Yellow
} catch {
    Write-Host "Warning: Docker is not running. You can still run the app locally if you have SQL Server and Redis installed." -ForegroundColor Yellow
}

Write-Host "`nChoose an option:" -ForegroundColor Cyan
Write-Host "1. Run with Docker Compose (recommended)" -ForegroundColor White
Write-Host "2. Run locally (requires SQL Server and Redis)" -ForegroundColor White
Write-Host "3. Build and test only" -ForegroundColor White

$choice = Read-Host "`nEnter your choice (1-3)"

switch ($choice) {
    "1" {
        Write-Host "`nStarting with Docker Compose..." -ForegroundColor Green
        docker-compose up -d
        Write-Host "`nApplication is starting up..." -ForegroundColor Green
        Write-Host "Blazor UI: http://localhost:5000" -ForegroundColor Yellow
        Write-Host "API: http://localhost:5001" -ForegroundColor Yellow
        Write-Host "SQL Server: localhost,1433" -ForegroundColor Yellow
        Write-Host "Redis: localhost:6379" -ForegroundColor Yellow
        Write-Host "`nTo stop the application, run: docker-compose down" -ForegroundColor Cyan
    }
    "2" {
        Write-Host "`nRunning locally..." -ForegroundColor Green
        Write-Host "Make sure SQL Server and Redis are running!" -ForegroundColor Yellow
        
        # Start API
        Write-Host "`nStarting API..." -ForegroundColor Green
        Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'src/BaratoInventory.API'; dotnet run"
        
        # Wait a bit for API to start
        Start-Sleep -Seconds 5
        
        # Start Blazor
        Write-Host "`nStarting Blazor..." -ForegroundColor Green
        Start-Process powershell -ArgumentList "-NoExit", "-Command", "cd 'src/BaratoInventory.Blazor'; dotnet run"
        
        Write-Host "`nApplication is starting up..." -ForegroundColor Green
        Write-Host "Blazor UI: http://localhost:5000" -ForegroundColor Yellow
        Write-Host "API: http://localhost:5001" -ForegroundColor Yellow
    }
    "3" {
        Write-Host "`nBuilding and testing..." -ForegroundColor Green
        
        # Build solution
        Write-Host "Building solution..." -ForegroundColor Yellow
        dotnet build
        
        # Run tests
        Write-Host "`nRunning tests..." -ForegroundColor Yellow
        dotnet test
        
        Write-Host "`nBuild and test completed!" -ForegroundColor Green
    }
    default {
        Write-Host "Invalid choice. Please run the script again." -ForegroundColor Red
    }
}

Write-Host "`nPress any key to exit..." -ForegroundColor Gray
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
