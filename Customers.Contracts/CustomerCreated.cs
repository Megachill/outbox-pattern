namespace Customers.Contracts;

public record CustomerCreated(
    Guid Id,
    string FullName,
    string Email,
    DateOnly DateOfBirth);
