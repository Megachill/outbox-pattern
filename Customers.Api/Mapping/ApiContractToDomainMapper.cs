using Customers.Api.Contracts.Requests;
using Customers.Api.Domain;

namespace Customers.Api.Mapping;

public static class ApiContractToDomainMapper
{
    public static Customer ToCustomer(this CustomerRequest request)
    {
        return new Customer
        {
            Email = request.Email,
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth
        };
    }

    public static Customer ToCustomer(this UpdateCustomerRequest request)
    {
        return new Customer
        {
            Id = request.Id,
            Email = request.Customer.Email,
            FullName = request.Customer.FullName,
            DateOfBirth = request.Customer.DateOfBirth
        };
    }
}
