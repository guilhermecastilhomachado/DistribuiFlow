using DistribuiFlow.Domain.Entities;
using DistribuiFlow.Domain.Enums;
using DistribuiFlow.Domain.Exceptions;

namespace DistribuiFlow.Tests.Domain;

public class OrderTests
{
    [Fact]
    public void Constructor_ShouldCreateOrderAsDraft()
    {
        var customerId = Guid.NewGuid();

        var order = new Order(customerId);

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(OrderStatus.Draft, order.Status);
        Assert.Empty(order.Items);
        Assert.Equal(0m, order.Total);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenCustomerIdIsEmpty()
    {
        Assert.Throws<DomainException>(() =>
            new Order(Guid.Empty));
    }

    [Fact]
    public void AddItem_ShouldAddProductToOrder()
    {
        var product = CreateProduct(
            price: 100m,
            stock: 10);

        var order = new Order(Guid.NewGuid());

        order.AddItem(product, 2);

        Assert.Single(order.Items);

        var item = order.Items.Single();

        Assert.Equal(product.Id, item.ProductId);
        Assert.Equal(2, item.Quantity);
        Assert.Equal(100m, item.UnitPrice);
        Assert.Equal(200m, item.Subtotal);
        Assert.Equal(200m, order.Total);
    }

    [Fact]
    public void AddItem_ShouldIncreaseQuantity_WhenProductAlreadyExists()
    {
        var product = CreateProduct(
            price: 100m,
            stock: 10);

        var order = new Order(Guid.NewGuid());

        order.AddItem(product, 2);
        order.AddItem(product, 3);

        Assert.Single(order.Items);

        var item = order.Items.Single();

        Assert.Equal(5, item.Quantity);
        Assert.Equal(500m, order.Total);
    }

    [Fact]
    public void AddItem_ShouldThrow_WhenProductIsInactive()
    {
        var product = CreateProduct();

        product.Deactivate();

        var order = new Order(Guid.NewGuid());

        Assert.Throws<DomainException>(() =>
            order.AddItem(product, 1));
    }

    [Fact]
    public void AddItem_ShouldThrow_WhenRequestedQuantityExceedsStock()
    {
        var product = CreateProduct(
            stock: 5);

        var order = new Order(Guid.NewGuid());

        Assert.Throws<DomainException>(() =>
            order.AddItem(product, 6));
    }

    [Fact]
    public void RemoveItem_ShouldRemoveExistingProduct()
    {
        var product = CreateProduct();

        var order = new Order(Guid.NewGuid());

        order.AddItem(product, 1);

        order.RemoveItem(product.Id);

        Assert.Empty(order.Items);
        Assert.Equal(0m, order.Total);
    }

    [Fact]
    public void Confirm_ShouldThrow_WhenOrderHasNoItems()
    {
        var order = new Order(Guid.NewGuid());

        Assert.Throws<DomainException>(() =>
            order.Confirm());

        Assert.Equal(
            OrderStatus.Draft,
            order.Status);
    }

    [Fact]
    public void Confirm_ShouldChangeStatus_WhenOrderHasItems()
    {
        var product = CreateProduct();

        var order = new Order(Guid.NewGuid());

        order.AddItem(product, 1);

        order.Confirm();

        Assert.Equal(
            OrderStatus.Confirmed,
            order.Status);
    }

    [Fact]
    public void AddItem_ShouldThrow_WhenOrderIsAlreadyConfirmed()
    {
        var product = CreateProduct(
            stock: 10);

        var order = new Order(Guid.NewGuid());

        order.AddItem(product, 1);

        order.Confirm();

        Assert.Throws<DomainException>(() =>
            order.AddItem(product, 1));
    }

    [Fact]
    public void Complete_ShouldChangeConfirmedOrderToCompleted()
    {
        var product = CreateProduct();

        var order = new Order(Guid.NewGuid());

        order.AddItem(product, 1);

        order.Confirm();
        order.Complete();

        Assert.Equal(
            OrderStatus.Completed,
            order.Status);
    }

    [Fact]
    public void Complete_ShouldThrow_WhenOrderIsStillDraft()
    {
        var order = new Order(Guid.NewGuid());

        Assert.Throws<DomainException>(() =>
            order.Complete());
    }

    [Fact]
    public void Cancel_ShouldChangeStatusToCancelled()
    {
        var order = new Order(Guid.NewGuid());

        order.Cancel();

        Assert.Equal(
            OrderStatus.Cancelled,
            order.Status);
    }

    [Fact]
    public void Cancel_ShouldThrow_WhenOrderIsCompleted()
    {
        var product = CreateProduct();

        var order = new Order(Guid.NewGuid());

        order.AddItem(product, 1);

        order.Confirm();
        order.Complete();

        Assert.Throws<DomainException>(() =>
            order.Cancel());

        Assert.Equal(
            OrderStatus.Completed,
            order.Status);
    }

    private static Product CreateProduct(
        decimal price = 100m,
        int stock = 10)
    {
        return new Product(
            "PROD-001",
            "Produto de teste",
            price,
            stock);
    }
}