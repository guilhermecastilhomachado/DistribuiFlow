using DistribuiFlow.Application.Interfaces.Repositories;
using DistribuiFlow.Domain.Entities;
using DistribuiFlow.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DistribuiFlow.Infrastructure.Repositories;

public sealed class ProductRepository : IProductRepository
{
    private readonly DistribuiFlowDbContext _dbContext;

    public ProductRepository(DistribuiFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(
                product => product.Id == id,
                cancellationToken);
    }

    public async Task<Product?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        var normalizedCode = code.Trim().ToUpperInvariant();

        return await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(
                product => product.Sku == normalizedCode,
                cancellationToken);
    }

    public async Task<IReadOnlyCollection<Product>> GetAllAsync(
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .OrderBy(product => product.Name)
            .ToArrayAsync(cancellationToken);
    }

    public async Task AddAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Products.AddAsync(
            product,
            cancellationToken);
    }

    public Task UpdateAsync(
        Product product,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Products.Update(product);

        return Task.CompletedTask;
    }
}