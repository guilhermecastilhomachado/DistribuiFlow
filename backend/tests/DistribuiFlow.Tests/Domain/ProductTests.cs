using DistribuiFlow.Domain.Entities;
using DistribuiFlow.Domain.Exceptions;

namespace DistribuiFlow.Tests.Domain;

public class ProductTests
{
    [Fact]
    public void Constructor_ShouldCreateProduct_WhenDataIsValid()
    {
        var product = new Product(
            "PROD-001",
            "Notebook",
            3500m,
            10);

        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.Equal("PROD-001", product.Sku);
        Assert.Equal("Notebook", product.Name);
        Assert.Equal(3500m, product.UnitPrice);
        Assert.Equal(10, product.StockQuantity);
        Assert.True(product.IsActive);
    }

    [Fact]
    public void Constructor_ShouldNormalizeSku()
    {
        var product = new Product(
            " prod-001 ",
            "Notebook",
            3500m,
            10);

        Assert.Equal("PROD-001", product.Sku);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenPriceIsInvalid()
    {
        var exception = Assert.Throws<DomainException>(() =>
            new Product(
                "PROD-001",
                "Notebook",
                -10m,
                5));

        Assert.Equal(
            "O preço do produto deve ser maior que zero.",
            exception.Message);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenInitialStockIsNegative()
    {
        Assert.Throws<DomainException>(() =>
            new Product(
                "PROD-001",
                "Notebook",
                100m,
                -1));
    }

    [Fact]
    public void IncreaseStock_ShouldAddQuantity()
    {
        var product = new Product(
            "PROD-001",
            "Notebook",
            3500m,
            10);

        product.IncreaseStock(5);

        Assert.Equal(15, product.StockQuantity);
    }

    [Fact]
    public void IncreaseStock_ShouldThrow_WhenQuantityIsZero()
    {
        var product = new Product(
            "PROD-001",
            "Notebook",
            3500m,
            10);

        Assert.Throws<DomainException>(() =>
            product.IncreaseStock(0));
    }

    [Fact]
    public void DecreaseStock_ShouldRemoveQuantity_WhenStockIsAvailable()
    {
        var product = new Product(
            "PROD-001",
            "Notebook",
            3500m,
            10);

        product.DecreaseStock(4);

        Assert.Equal(6, product.StockQuantity);
    }

    [Fact]
    public void DecreaseStock_ShouldThrow_WhenStockIsInsufficient()
    {
        var product = new Product(
            "PROD-001",
            "Notebook",
            3500m,
            5);

        Assert.Throws<DomainException>(() =>
            product.DecreaseStock(6));

        Assert.Equal(5, product.StockQuantity);
    }

    [Fact]
    public void Deactivate_ShouldMarkProductAsInactive()
    {
        var product = new Product(
            "PROD-001",
            "Notebook",
            3500m,
            10);

        product.Deactivate();

        Assert.False(product.IsActive);
    }
}