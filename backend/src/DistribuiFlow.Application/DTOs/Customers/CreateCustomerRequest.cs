namespace DistribuiFlow.Application.DTOs.Customers;

public sealed record CreateCustomerRequest(
    string Name,
    string Email
);