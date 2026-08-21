using DistribuiFlow.Domain.Entities;
using DistribuiFlow.Domain.Exceptions;

namespace DistribuiFlow.Tests.Domain;

public class CustomerTests
{
    [Fact]
    public void Constructor_ShouldCreateCustomer_WhenDataIsValid()
    {
        var customer = new Customer(
            "João Silva",
            "12345678900",
            "joao@email.com");

        Assert.NotEqual(Guid.Empty, customer.Id);
        Assert.Equal("João Silva", customer.Name);
        Assert.Equal("12345678900", customer.Document);
        Assert.Equal("joao@email.com", customer.Email);
        Assert.True(customer.IsActive);
    }

    [Fact]
    public void Constructor_ShouldNormalizeEmail()
    {
        var customer = new Customer(
            "João Silva",
            "12345678900",
            " JOAO@EMAIL.COM ");

        Assert.Equal(
            "joao@email.com",
            customer.Email);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenNameIsEmpty()
    {
        Assert.Throws<DomainException>(() =>
            new Customer(
                "",
                "12345678900",
                "joao@email.com"));
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenDocumentIsEmpty()
    {
        Assert.Throws<DomainException>(() =>
            new Customer(
                "João Silva",
                "",
                "joao@email.com"));
    }

    [Fact]
    public void UpdateContact_ShouldUpdateNameAndEmail()
    {
        var customer = new Customer(
            "João Silva",
            "12345678900",
            "joao@email.com");

        customer.UpdateContact(
            "João da Silva",
            "NOVO@EMAIL.COM");

        Assert.Equal(
            "João da Silva",
            customer.Name);

        Assert.Equal(
            "novo@email.com",
            customer.Email);
    }

    [Fact]
    public void Deactivate_ShouldMarkCustomerAsInactive()
    {
        var customer = new Customer(
            "João Silva",
            "12345678900",
            "joao@email.com");

        customer.Deactivate();

        Assert.False(customer.IsActive);
    }
}