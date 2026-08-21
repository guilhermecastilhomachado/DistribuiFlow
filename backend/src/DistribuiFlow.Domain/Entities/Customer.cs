using DistribuiFlow.Domain.Exceptions;

namespace DistribuiFlow.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Document { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    private Customer()
    {
    }

    public Customer(
        string name,
        string document,
        string email)
    {
        ValidateRequiredField(name, "nome");
        ValidateRequiredField(document, "documento");
        ValidateRequiredField(email, "e-mail");

        Id = Guid.NewGuid();

        Name = name.Trim();
        Document = document.Trim();
        Email = email.Trim().ToLowerInvariant();

        IsActive = true;

        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public void UpdateContact(string name, string email)
    {
        ValidateRequiredField(name, "nome");
        ValidateRequiredField(email, "e-mail");

        Name = name.Trim();
        Email = email.Trim().ToLowerInvariant();

        Touch();
    }

    public void Deactivate()
    {
        IsActive = false;

        Touch();
    }

    private static void ValidateRequiredField(
        string value,
        string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(
                $"O campo {fieldName} é obrigatório.");
        }
    }

    private void Touch()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}