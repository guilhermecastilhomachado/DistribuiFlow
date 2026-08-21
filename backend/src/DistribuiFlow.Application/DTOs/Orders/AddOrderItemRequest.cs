namespace DistribuiFlow.Application.DTOs.Orders;

public sealed record AddOrderItemRequest(
    Guid ProductId,
    int Quantity
);