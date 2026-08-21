namespace DistribuiFlow.Application.DTOs.Orders;

public sealed record OrderResponse(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    string Status,
    decimal Total,
    IReadOnlyCollection<OrderItemResponse> Items
);