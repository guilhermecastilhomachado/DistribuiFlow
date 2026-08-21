namespace DistribuiFlow.Application.DTOs.Products;

public sealed record ProductResponse(
    Guid Id,
    string Code,
    string Name,
    decimal Price,
    int Stock,
    bool IsActive
);