namespace Customers.Api.Contracts.Requests;

public class CustomerRequest
{
    public string FullName { get; init; } = default!;

    public string Email { get; init; } = default!;

    public DateOnly DateOfBirth { get; init; } = default!;
}
