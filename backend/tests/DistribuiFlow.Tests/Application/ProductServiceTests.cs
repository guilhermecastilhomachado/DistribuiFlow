using DistribuiFlow.Application.DTOs.Products;
using DistribuiFlow.Application.Exceptions;
using DistribuiFlow.Application.Services;
using DistribuiFlow.Domain.Entities;
using DistribuiFlow.Domain.Exceptions;
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

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct_WhenProductExists()
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

        var request = new UpdateProductRequest(
            "Notebook Pro",
            4200m);

        var response = await service.UpdateAsync(
            product.Id,
            request);

        Assert.Equal("Notebook Pro", response.Name);
        Assert.Equal(4200m, response.Price);

        Assert.Equal("Notebook Pro", product.Name);
        Assert.Equal(4200m, product.UnitPrice);

        Assert.Equal(1, repository.UpdateCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowNotFound_WhenProductDoesNotExist()
    {
        var repository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var service = new ProductService(
            repository,
            unitOfWork);

        var request = new UpdateProductRequest(
            "Notebook Pro",
            4200m);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.UpdateAsync(
                Guid.NewGuid(),
                request));

        Assert.Equal(0, repository.UpdateCallCount);
        Assert.Equal(0, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task IncreaseStockAsync_ShouldIncreaseStock_WhenProductExists()
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

        var request = new ChangeStockRequest(5);

        var response = await service.IncreaseStockAsync(
            product.Id,
            request);

        Assert.Equal(15, response.Stock);
        Assert.Equal(15, product.StockQuantity);

        Assert.Equal(1, repository.UpdateCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task IncreaseStockAsync_ShouldThrowDomainException_WhenQuantityIsInvalid()
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

        var request = new ChangeStockRequest(0);

        await Assert.ThrowsAsync<DomainException>(
            () => service.IncreaseStockAsync(
                product.Id,
                request));

        Assert.Equal(10, product.StockQuantity);
        Assert.Equal(0, repository.UpdateCallCount);
        Assert.Equal(0, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task IncreaseStockAsync_ShouldThrowNotFound_WhenProductDoesNotExist()
    {
        var repository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var service = new ProductService(
            repository,
            unitOfWork);

        var request = new ChangeStockRequest(5);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.IncreaseStockAsync(
                Guid.NewGuid(),
                request));

        Assert.Equal(0, repository.UpdateCallCount);
        Assert.Equal(0, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task DecreaseStockAsync_ShouldDecreaseStock_WhenProductExists()
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

        var request = new ChangeStockRequest(4);

        var response = await service.DecreaseStockAsync(
            product.Id,
            request);

        Assert.Equal(6, response.Stock);
        Assert.Equal(6, product.StockQuantity);

        Assert.Equal(1, repository.UpdateCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task DecreaseStockAsync_ShouldThrowDomainException_WhenStockIsInsufficient()
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

        var request = new ChangeStockRequest(11);

        await Assert.ThrowsAsync<DomainException>(
            () => service.DecreaseStockAsync(
                product.Id,
                request));

        Assert.Equal(10, product.StockQuantity);
        Assert.Equal(0, repository.UpdateCallCount);
        Assert.Equal(0, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task DecreaseStockAsync_ShouldThrowNotFound_WhenProductDoesNotExist()
    {
        var repository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var service = new ProductService(
            repository,
            unitOfWork);

        var request = new ChangeStockRequest(2);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.DecreaseStockAsync(
                Guid.NewGuid(),
                request));

        Assert.Equal(0, repository.UpdateCallCount);
        Assert.Equal(0, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task DeactivateAsync_ShouldDeactivateProduct_WhenProductExists()
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

        var response = await service.DeactivateAsync(
            product.Id);

        Assert.False(response.IsActive);
        Assert.False(product.IsActive);

        Assert.Equal(1, repository.UpdateCallCount);
        Assert.Equal(1, unitOfWork.SaveChangesCallCount);
    }

    [Fact]
    public async Task DeactivateAsync_ShouldThrowNotFound_WhenProductDoesNotExist()
    {
        var repository = new FakeProductRepository();
        var unitOfWork = new FakeUnitOfWork();

        var service = new ProductService(
            repository,
            unitOfWork);

        await Assert.ThrowsAsync<NotFoundException>(
            () => service.DeactivateAsync(
                Guid.NewGuid()));

        Assert.Equal(0, repository.UpdateCallCount);
        Assert.Equal(0, unitOfWork.SaveChangesCallCount);
    }
}