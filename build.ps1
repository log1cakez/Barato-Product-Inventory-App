# Barato Product Inventory App - Build Script
Write-Host "Building Barato Product Inventory App..." -ForegroundColor Green

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
dotnet clean

# Restore packages
Write-Host "Restoring packages..." -ForegroundColor Yellow
dotnet restore

# Build solution
Write-Host "Building solution..." -ForegroundColor Yellow
dotnet build --configuration Release --no-restore

# Run tests
Write-Host "Running tests..." -ForegroundColor Yellow
dotnet test --configuration Release --no-build

Write-Host "`nBuild completed successfully!" -ForegroundColor Green
Write-Host "To run the application:" -ForegroundColor Cyan
Write-Host "  - With Docker: docker-compose up -d" -ForegroundColor White
Write-Host "  - Locally: .\run-app.ps1" -ForegroundColor White
