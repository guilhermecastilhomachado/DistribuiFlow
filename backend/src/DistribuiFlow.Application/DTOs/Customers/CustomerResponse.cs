namespace DistribuiFlow.Application.DTOs.Customers;

public sealed record CustomerResponse(
    Guid Id,
    string Name,
    string Email,
    bool IsActive
);