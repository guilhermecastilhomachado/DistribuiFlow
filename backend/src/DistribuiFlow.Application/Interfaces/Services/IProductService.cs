using DistribuiFlow.Application.DTOs.Products;

namespace DistribuiFlow.Application.Interfaces.Services;

public interface IProductService
{
    Task<ProductResponse> CreateAsync(
        CreateProductRequest request,
        CancellationToken cancellationToken = default);

    Task<ProductResponse> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<ProductResponse>> GetAllAsync(
        CancellationToken cancellationToken = default);

    Task<ProductResponse> UpdateAsync(
        Guid id,
        UpdateProductRequest request,
        CancellationToken cancellationToken = default);

    Task<ProductResponse> IncreaseStockAsync(
        Guid id,
        ChangeStockRequest request,
        CancellationToken cancellationToken = default);

    Task<ProductResponse> DecreaseStockAsync(
        Guid id,
        ChangeStockRequest request,
        CancellationToken cancellationToken = default);

    Task<ProductResponse> DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken = default);
}