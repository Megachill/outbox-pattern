namespace Customers.Contracts;

public record CustomerUpdated(
    Guid Id,
    string FullName,
    string Email,
    DateOnly DateOfBirth);
