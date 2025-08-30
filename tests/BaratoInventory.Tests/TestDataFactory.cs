using BaratoInventory.Core.Entities;

namespace BaratoInventory.Tests;

public static class TestDataFactory
{
    public static Product CreateValidProduct(string name = "Test Product", string category = "Electronics", decimal price = 99.99m, int quantity = 10)
    {
        return new Product
        {
            Name = name,
            Category = category,
            Price = price,
            Quantity = quantity
        };
    }

    public static Product CreateLaptop()
    {
        return CreateValidProduct("Laptop", "Electronics", 999.99m, 5);
    }

    public static Product CreateMouse()
    {
        return CreateValidProduct("Mouse", "Electronics", 29.99m, 50);
    }

    public static Product CreateChair()
    {
        return CreateValidProduct("Desk Chair", "Furniture", 199.99m, 15);
    }

    public static Product CreateMug()
    {
        return CreateValidProduct("Coffee Mug", "Kitchen", 12.99m, 100);
    }

    public static List<Product> CreateSampleProducts()
    {
        return new List<Product>
        {
            CreateLaptop(),
            CreateMouse(),
            CreateChair(),
            CreateMug()
        };
    }

    public static Product CreateInvalidProduct()
    {
        return new Product
        {
            Name = "",
            Category = "",
            Price = 0m,
            Quantity = -1
        };
    }

    public static Product CreateProductWithLongName()
    {
        return new Product
        {
            Name = new string('A', 101),
            Category = "Electronics",
            Price = 99.99m,
            Quantity = 10
        };
    }

    public static Product CreateProductWithLongCategory()
    {
        return new Product
        {
            Name = "Test Product",
            Category = new string('A', 51),
            Price = 99.99m,
            Quantity = 10
        };
    }
}
