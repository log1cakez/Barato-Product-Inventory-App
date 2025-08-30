# Unit Testing Guide for Barato Product Inventory

## 🧪 Overview

This project uses **NUnit** as the testing framework and **Moq** for mocking dependencies. The tests are organized to cover all layers of the application: entities, repositories, services, and infrastructure.

## 🚀 Running Tests

### From Command Line

```bash
# Run all tests
dotnet test

# Run tests with verbose output
dotnet test --verbosity normal

# Run specific test project
dotnet test tests/BaratoInventory.Tests/

# Run tests with coverage (if coverlet is installed)
dotnet test --collect:"XPlat Code Coverage"
```

### From Visual Studio

1. Open Test Explorer (Test → Test Explorer)
2. Click "Run All" to run all tests
3. Right-click specific tests to run individual tests

### From VS Code

1. Install .NET Core Test Explorer extension
2. Open Command Palette (Ctrl+Shift+P)
3. Type ".NET Core Test Explorer: Run All Tests"

## 📁 Test Structure

```
tests/BaratoInventory.Tests/
├── ProductServiceTests.cs      # Business logic tests
├── ProductEntityTests.cs       # Entity validation tests
├── ProductRepositoryTests.cs   # Data access tests
├── RedisCacheServiceTests.cs   # Cache service tests
├── TestBase.cs                 # Common test utilities
├── TestDataFactory.cs          # Test data creation
└── README.md                   # This file
```

## 🎯 Test Categories

### 1. **Entity Tests** (`ProductEntityTests.cs`)

- **Purpose**: Test data validation and business rules
- **What to test**:
  - Required field validation
  - String length constraints
  - Numeric range validation
  - Default value assignments
- **Example**:

```csharp
[Test]
public void Product_WithValidData_ShouldBeValid()
{
    var product = TestDataFactory.CreateValidProduct();
    var validationResults = new List<ValidationResult>();
    var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);

    Assert.That(isValid, Is.True);
    Assert.That(validationResults, Is.Empty);
}
```

### 2. **Repository Tests** (`ProductRepositoryTests.cs`)

- **Purpose**: Test data access layer
- **What to test**:
  - CRUD operations
  - Search functionality
  - Database interactions
- **Uses**: In-memory database for fast, isolated tests
- **Example**:

```csharp
[Test]
public async Task GetAllAsync_WithProducts_ShouldReturnAllProducts()
{
    var products = TestDataFactory.CreateSampleProducts();
    await _context.Products.AddRangeAsync(products);
    await _context.SaveChangesAsync();

    var result = await _repository.GetAllAsync();

    Assert.That(result.Count(), Is.EqualTo(4));
}
```

### 3. **Service Tests** (`ProductServiceTests.cs`)

- **Purpose**: Test business logic and orchestration
- **What to test**:
  - Service method behavior
  - Cache interactions
  - Repository calls
  - Error handling
- **Uses**: Moq for mocking dependencies
- **Example**:

```csharp
[Test]
public async Task GetAllProductsAsync_WhenCacheHit_ReturnsCachedProducts()
{
    var expectedProducts = TestDataFactory.CreateSampleProducts();
    _mockCacheService.Setup(x => x.GetAsync<IEnumerable<Product>>(It.IsAny<string>()))
        .ReturnsAsync(expectedProducts);

    var result = await _productService.GetAllProductsAsync();

    Assert.That(result, Is.EqualTo(expectedProducts));
    _mockRepository.Verify(x => x.GetAllAsync(), Times.Never);
}
```

### 4. **Infrastructure Tests** (`RedisCacheServiceTests.cs`)

- **Purpose**: Test external service integrations
- **What to test**:
  - Service method calls
  - Error handling
  - Parameter validation
- **Uses**: Moq for mocking external dependencies

## 🛠️ Test Utilities

### TestBase Class

Provides common setup methods for database tests:

```csharp
public class ProductRepositoryTests : TestBase
{
    [SetUp]
    public void Setup()
    {
        _context = CreateContext();
        _repository = new ProductRepository(_context);
    }

    [TearDown]
    public async Task TearDown()
    {
        await CleanupContextAsync(_context);
    }
}
```

### TestDataFactory

Creates consistent test data:

```csharp
var product = TestDataFactory.CreateLaptop();
var products = TestDataFactory.CreateSampleProducts();
var invalidProduct = TestDataFactory.CreateInvalidProduct();
```

## 📝 Writing New Tests

### 1. **Follow Naming Convention**

```csharp
[Test]
public void MethodName_WhenCondition_ShouldExpectedBehavior()
{
    // Test implementation
}
```

### 2. **Use Arrange-Act-Assert Pattern**

```csharp
[Test]
public async Task CreateProduct_WithValidData_ShouldSaveToDatabase()
{
    // Arrange
    var product = TestDataFactory.CreateValidProduct();

    // Act
    var result = await _service.CreateProductAsync(product);

    // Assert
    Assert.That(result.Id, Is.GreaterThan(0));
    Assert.That(result.Name, Is.EqualTo(product.Name));
}
```

### 3. **Test Both Happy Path and Edge Cases**

```csharp
[Test]
public void CreateProduct_WithNullProduct_ShouldThrowArgumentNullException()
{
    Assert.ThrowsAsync<ArgumentNullException>(async () =>
        await _service.CreateProductAsync(null));
}
```

### 4. **Use Descriptive Test Names**

- ❌ `Test1()`
- ✅ `CreateProduct_WithValidData_ShouldReturnProductWithId()`

## 🔧 Test Configuration

### appsettings.test.json

Create test-specific configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=BaratoInventory_Test;Trusted_Connection=true;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  }
}
```

### Test Categories

Use categories to organize tests:

```csharp
[Test]
[Category("Integration")]
public async Task CreateProduct_EndToEnd_ShouldWork()
{
    // Integration test
}

[Test]
[Category("Unit")]
public void ValidateProduct_WithInvalidData_ShouldFail()
{
    // Unit test
}
```

## 📊 Test Coverage

### Install Coverlet

```bash
dotnet tool install --global coverlet.collector
```

### Run with Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### View Coverage Report

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
reportgenerator -reports:coverage.cobertura.xml -targetdir:coverage
```

## 🚨 Common Issues

### 1. **Test Isolation**

- Each test should be independent
- Use `[SetUp]` and `[TearDown]` for cleanup
- Use unique database names for in-memory tests

### 2. **Async Tests**

- Always use `async Task` for async test methods
- Use `Assert.ThrowsAsync<T>()` for testing exceptions

### 3. **Mocking**

- Mock only external dependencies
- Test the behavior, not the implementation
- Use `Times.Once()`, `Times.Never()` for verification

### 4. **Database Tests**

- Use in-memory database for fast tests
- Clean up after each test
- Don't test EF Core itself, test your repository logic

## 📚 Best Practices

1. **Test One Thing**: Each test should verify one specific behavior
2. **Fast Execution**: Tests should run quickly (under 100ms each)
3. **No External Dependencies**: Tests should not require database, network, or file system
4. **Clear Assertions**: Use descriptive assertion messages
5. **Maintainable**: Tests should be easy to understand and modify
6. **Coverage**: Aim for 80%+ code coverage

## 🔍 Debugging Tests

### Run Single Test

```bash
dotnet test --filter "FullyQualifiedName~ProductServiceTests.CreateProduct_WithValidData_ShouldSaveToDatabase"
```

### Debug Mode

```bash
dotnet test --logger "console;verbosity=detailed"
```

### Break on Failures

```bash
dotnet test --logger "console;verbosity=detailed" --stop-on-fail
```

## 📈 Continuous Integration

### GitHub Actions Example

```yaml
- name: Run Tests
  run: dotnet test --verbosity normal --collect:"XPlat Code Coverage"
```

### Azure DevOps Example

```yaml
- task: DotNetCoreCLI@2
  inputs:
    command: "test"
    projects: "**/*Tests/*.csproj"
    arguments: '--collect:"XPlat Code Coverage"'
```

## 🎉 Success Metrics

- **Test Count**: Aim for 2-3 tests per method
- **Coverage**: 80%+ line coverage
- **Execution Time**: All tests run in under 30 seconds
- **Reliability**: 100% pass rate on CI/CD

---

**Happy Testing! 🧪✨**
