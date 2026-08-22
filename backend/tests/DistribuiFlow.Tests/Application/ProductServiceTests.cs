using DistribuiFlow.Application.DTOs.Products;
using DistribuiFlow.Application.Exceptions;
using DistribuiFlow.Application.Services;
using DistribuiFlow.Domain.Entities;
using DistribuiFlow.Tests.Fakes;

namespace DistribuiFlow.Tests.Application;

public sealed class ProductServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateProduct_WhenCodeDoesNotExist()
    {
        var repository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var service = new ProductService(
            repository,
            unitOfWork);

        var request = new CreateProductRequest(
            "PROD-001",
            "Notebook",
            3500m,
            10);

        var response = await service.CreateAsync(request);

        Assert.Equal("PROD-001", response.Code);
        Assert.Equal("Notebook", response.Name);
        Assert.Equal(3500m, response.Price);
        Assert.Equal(10, response.Stock);
        Assert.True(response.IsActive);

        Assert.Single(repository.Products);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowConflict_WhenCodeAlreadyExists()
    {
        var repository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        repository.Seed(
            new Product(
                "PROD-001",
                "Notebook",
                3500m,
                10));

        var service = new ProductService(
            repository,
            unitOfWork);

        var request = new CreateProductRequest(
            "PROD-001",
            "Outro notebook",
            4000m,
            5);

        await Assert.ThrowsAsync<ConflictException>(
            () => service.CreateAsync(request));

        Assert.Single(repository.Products);
        Assert.Equal(0, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExists()
    {
        var repository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var product = new Product(
            "PROD-001",
            "Notebook",
            3500m,
            10);

        repository.Seed(product);

        var service = new ProductService(
            repository,
            unitOfWork);

        var response =
            await service.GetByIdAsync(product.Id);

        Assert.Equal(product.Id, response.Id);
        Assert.Equal(product.Sku, response.Code);
        Assert.Equal(product.Name, response.Name);
        Assert.Equal(product.UnitPrice, response.Price);
        Assert.Equal(product.StockQuantity, response.Stock);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldThrowNotFound_WhenProductDoesNotExist()
    {
        var repository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var service = new ProductService(
            repository,
            unitOfWork);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.GetByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllProducts()
    {
        var repository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        repository.Seed(
            new Product(
                "PROD-001",
                "Produto 1",
                100m,
                10));

        repository.Seed(
            new Product(
                "PROD-002",
                "Produto 2",
                200m,
                20));

        var service = new ProductService(
            repository,
            unitOfWork);

        var products =
            await service.GetAllAsync();

        Assert.Equal(2, products.Count);
    }
}