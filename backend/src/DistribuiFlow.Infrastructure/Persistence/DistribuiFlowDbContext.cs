using DistribuiFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DistribuiFlow.Infrastructure.Persistence;

public sealed class DistribuiFlowDbContext : DbContext
{
    public DistribuiFlowDbContext(
        DbContextOptions<DistribuiFlowDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Order> Orders => Set<Order>();

    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(DistribuiFlowDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}