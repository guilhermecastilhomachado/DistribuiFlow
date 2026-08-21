namespace DistribuiFlow.Application.DTOs.Products;

public sealed record CreateProductRequest(
    string Code,
    string Name,
    decimal Price,
    int Stock
);