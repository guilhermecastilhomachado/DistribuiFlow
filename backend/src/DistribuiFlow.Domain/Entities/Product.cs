using DistribuiFlow.Domain.Exceptions;

namespace DistribuiFlow.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Sku { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int StockQuantity { get; private set; }
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    private Product()
    {
    }

    public Product(
        string sku,
        string name,
        decimal unitPrice,
        int initialStock = 0)
    {
        ValidateSku(sku);
        ValidateName(name);
        ValidatePrice(unitPrice);

        if (initialStock < 0)
        {
            throw new DomainException(
                "O estoque inicial não pode ser negativo.");
        }

        Id = Guid.NewGuid();
        Sku = sku.Trim().ToUpperInvariant();
        Name = name.Trim();
        UnitPrice = unitPrice;
        StockQuantity = initialStock;
        IsActive = true;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void UpdateDetails(string name, decimal unitPrice)
    {
        ValidateName(name);
        ValidatePrice(unitPrice);

        Name = name.Trim();
        UnitPrice = unitPrice;

        Touch();
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException(
                "A quantidade adicionada ao estoque deve ser maior que zero.");
        }

        StockQuantity += quantity;

        Touch();
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException(
                "A quantidade retirada do estoque deve ser maior que zero.");
        }

        if (quantity > StockQuantity)
        {
            throw new DomainException(
                "Não há estoque suficiente para realizar a operação.");
        }

        StockQuantity -= quantity;

        Touch();
    }

    public bool HasStock(int quantity)
    {
        return quantity > 0 && StockQuantity >= quantity;
    }

    public void Deactivate()
    {
        IsActive = false;

        Touch();
    }

    private static void ValidateSku(string sku)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new DomainException(
                "O SKU do produto é obrigatório.");
        }
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException(
                "O nome do produto é obrigatório.");
        }
    }

    private static void ValidatePrice(decimal unitPrice)
    {
        if (unitPrice <= 0)
        {
            throw new DomainException(
                "O preço do produto deve ser maior que zero.");
        }
    }

    private void Touch()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}