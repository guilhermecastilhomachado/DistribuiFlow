namespace DistribuiFlow.Application.DTOs.Orders;

public sealed record CreateOrderRequest(
    Guid CustomerId
);