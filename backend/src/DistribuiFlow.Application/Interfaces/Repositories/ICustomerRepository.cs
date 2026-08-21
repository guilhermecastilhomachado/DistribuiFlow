using DistribuiFlow.Domain.Entities;

namespace DistribuiFlow.Application.Interfaces.Repositories;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<Customer?> GetByEmailAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<Customer>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Customer customer,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(
        Customer customer,
        CancellationToken cancellationToken = default);
}