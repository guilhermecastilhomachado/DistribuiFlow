using DistribuiFlow.Domain.Enums;
using DistribuiFlow.Domain.Exceptions;

namespace DistribuiFlow.Domain.Entities;

public class Order
{
    private readonly List<OrderItem> _items = new();

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public IReadOnlyCollection<OrderItem> Items =>
        _items.AsReadOnly();

    public decimal Total =>
        _items.Sum(item => item.Subtotal);

    private Order()
    {
    }

    public Order(Guid customerId)
    {
        if (customerId == Guid.Empty)
        {
            throw new DomainException(
                "O cliente do pedido é obrigatório.");
        }

        Id = Guid.NewGuid();
        CustomerId = customerId;

        Status = OrderStatus.Draft;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void AddItem(Product product, int quantity)
    {
        if (product is null)
        {
            throw new DomainException(
                "O produto é obrigatório.");
        }

        EnsureDraft();

        if (!product.IsActive)
        {
            throw new DomainException(
                "Não é possível adicionar um produto inativo ao pedido.");
        }

        if (quantity <= 0)
        {
            throw new DomainException(
                "A quantidade deve ser maior que zero.");
        }

        var existingItem =
            _items.FirstOrDefault(
                item => item.ProductId == product.Id);

        var finalQuantity =
            quantity + (existingItem?.Quantity ?? 0);

        if (!product.HasStock(finalQuantity))
        {
            throw new DomainException(
                "Não há estoque suficiente para adicionar esta quantidade.");
        }

        if (existingItem is not null)
        {
            existingItem.IncreaseQuantity(quantity);
        }
        else
        {
            _items.Add(
                new OrderItem(
                    product.Id,
                    product.Name,
                    product.UnitPrice,
                    quantity));
        }

        Touch();
    }

    public void RemoveItem(Guid productId)
    {
        EnsureDraft();

        var item =
            _items.FirstOrDefault(
                item => item.ProductId == productId);

        if (item is null)
        {
            throw new DomainException(
                "O produto não existe neste pedido.");
        }

        _items.Remove(item);

        Touch();
    }

    public void Confirm()
    {
        EnsureDraft();

        if (_items.Count == 0)
        {
            throw new DomainException(
                "Não é possível confirmar um pedido sem itens.");
        }

        Status = OrderStatus.Confirmed;

        Touch();
    }

    public void Complete()
    {
        if (Status != OrderStatus.Confirmed)
        {
            throw new DomainException(
                "Somente pedidos confirmados podem ser concluídos.");
        }

        Status = OrderStatus.Completed;

        Touch();
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Completed)
        {
            throw new DomainException(
                "Um pedido concluído não pode ser cancelado.");
        }

        if (Status == OrderStatus.Cancelled)
        {
            throw new DomainException(
                "O pedido já está cancelado.");
        }

        Status = OrderStatus.Cancelled;

        Touch();
    }

    private void EnsureDraft()
    {
        if (Status != OrderStatus.Draft)
        {
            throw new DomainException(
                "O pedido só pode ser alterado enquanto estiver em rascunho.");
        }
    }

    private void Touch()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}