using DistribuiFlow.Application.Interfaces;

namespace DistribuiFlow.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly DistribuiFlowDbContext _dbContext;

    public UnitOfWork(DistribuiFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}