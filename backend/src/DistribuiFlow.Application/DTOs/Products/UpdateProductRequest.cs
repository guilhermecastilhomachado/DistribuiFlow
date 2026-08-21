namespace DistribuiFlow.Application.DTOs.Products;

public sealed record UpdateProductRequest(
    string Name,
    decimal Price
);