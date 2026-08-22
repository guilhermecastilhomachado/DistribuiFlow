using DistribuiFlow.Application.Interfaces.Repositories;
using DistribuiFlow.Domain.Entities;

namespace DistribuiFlow.Tests.Fakes;

public sealed class FakeProductRepository : IProductRepository
{
    private readonly List<Product> _products = [];

    public IReadOnlyCollection<Product> Products =>
        _products.AsReadOnly();

    public Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var product = _products.FirstOrDefault(
            product => product.Id == id);

        return Task.FromResult(product);
    }

    public Task<Product?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode =
            code.Trim().ToUpperInvariant();

        var product = _products.FirstOrDefault(
            product => product.Sku == normalizedCode);

        return Task.FromResult(product);
    }

    public Task<IReadOnlyCollection<Product>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<Product>>(
            _products.ToArray());
    }

    public Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        _products.Add(product);

        return Task.CompletedTask;
    }

    public Task UpdateAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public void Seed(Product product)
    {
        _products.Add(product);
    }
}