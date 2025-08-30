using System.ComponentModel.DataAnnotations;
using BaratoInventory.Core.Entities;
using NUnit.Framework;

namespace BaratoInventory.Tests;

[TestFixture]
public class ProductEntityTests
{
    [Test]
    public void Product_WithValidData_ShouldBeValid()
    {
        var product = new Product
        {
            Name = "Test Product",
            Category = "Electronics",
            Price = 99.99m,
            Quantity = 10
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);

        Assert.That(isValid, Is.True);
        Assert.That(validationResults, Is.Empty);
    }

    [Test]
    public void Product_WithEmptyName_ShouldBeInvalid()
    {
        var product = new Product
        {
            Name = "",
            Category = "Electronics",
            Price = 99.99m,
            Quantity = 10
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);

        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Count, Is.GreaterThan(0));
        Assert.That(validationResults.Any(v => v.MemberNames.Contains("Name")), Is.True);
    }

    [Test]
    public void Product_WithNameTooLong_ShouldBeInvalid()
    {
        var product = new Product
        {
            Name = new string('A', 101),
            Category = "Electronics",
            Price = 99.99m,
            Quantity = 10
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);

        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Any(v => v.MemberNames.Contains("Name")), Is.True);
    }

    [Test]
    public void Product_WithZeroPrice_ShouldBeInvalid()
    {
        var product = new Product
        {
            Name = "Test Product",
            Category = "Electronics",
            Price = 0m,
            Quantity = 10
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);

        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Any(v => v.MemberNames.Contains("Price")), Is.True);
    }

    [Test]
    public void Product_WithNegativeQuantity_ShouldBeInvalid()
    {
        var product = new Product
        {
            Name = "Test Product",
            Category = "Electronics",
            Price = 99.99m,
            Quantity = -1
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);

        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Any(v => v.MemberNames.Contains("Quantity")), Is.True);
    }

    [Test]
    public void Product_WithEmptyCategory_ShouldBeInvalid()
    {
        var product = new Product
        {
            Name = "Test Product",
            Category = "",
            Price = 99.99m,
            Quantity = 10
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);

        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Any(v => v.MemberNames.Contains("Category")), Is.True);
    }

    [Test]
    public void Product_WithCategoryTooLong_ShouldBeInvalid()
    {
        var product = new Product
        {
            Name = "Test Product",
            Category = new string('A', 51),
            Price = 99.99m,
            Quantity = 10
        };

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(product, new ValidationContext(product), validationResults, true);

        Assert.That(isValid, Is.False);
        Assert.That(validationResults.Any(v => v.MemberNames.Contains("Category")), Is.True);
    }

    [Test]
    public void Product_CreatedAt_ShouldBeSetToUtcNow()
    {
        var beforeCreation = DateTime.UtcNow;
        var product = new Product();
        var afterCreation = DateTime.UtcNow;

        Assert.That(product.CreatedAt, Is.GreaterThanOrEqualTo(beforeCreation));
        Assert.That(product.CreatedAt, Is.LessThanOrEqualTo(afterCreation));
    }

    [Test]
    public void Product_UpdatedAt_ShouldBeNullInitially()
    {
        var product = new Product();
        Assert.That(product.UpdatedAt, Is.Null);
    }
}
