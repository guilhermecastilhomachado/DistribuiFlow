using DistribuiFlow.Domain.Exceptions;

namespace DistribuiFlow.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }

    public decimal Subtotal => UnitPrice * Quantity;

    private OrderItem()
    {
    }

    internal OrderItem(
        Guid productId,
        string productName,
        decimal unitPrice,
        int quantity)
    {
        if (productId == Guid.Empty)
        {
            throw new DomainException(
                "O produto do item é obrigatório.");
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new DomainException(
                "O nome do produto é obrigatório.");
        }

        if (unitPrice <= 0)
        {
            throw new DomainException(
                "O preço unitário deve ser maior que zero.");
        }

        if (quantity <= 0)
        {
            throw new DomainException(
                "A quantidade do item deve ser maior que zero.");
        }

        Id = Guid.NewGuid();
        ProductId = productId;
        ProductName = productName.Trim();
        UnitPrice = unitPrice;
        Quantity = quantity;
    }

    internal void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException(
                "A quantidade adicionada deve ser maior que zero.");
        }

        Quantity += quantity;
    }
}