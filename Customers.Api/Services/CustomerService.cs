using Customers.Api.Data;
using Customers.Api.Domain;
using Customers.Contracts;
using FluentValidation;
using FluentValidation.Results;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Customers.Api.Services;

public class CustomerService : ICustomerService
{
    private readonly AppDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public CustomerService(AppDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> CreateAsync(Customer customer)
    {
        var existingUser = await _dbContext.Customers.FindAsync(customer.Id);
        
        if (existingUser is not null)
        {
            var errorMessage = $"A user with id {customer.Id} already exists";
            throw new ValidationException(errorMessage, GenerateValidationError(errorMessage));
        }

        await _dbContext.Customers.AddAsync(customer);
        
        var message = new CustomerCreated(customer.Id, customer.FullName, customer.Email, customer.DateOfBirth);
        await _publishEndpoint.Publish(message);
        
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<Customer?> GetAsync(Guid id)
    {
        return await _dbContext.Customers.FindAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        return await _dbContext.Customers.ToListAsync();
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        _dbContext.Customers.Update(customer);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await _dbContext.Customers.FindAsync(id);
        if (customer is null)
        {
            return false;
        }

        _dbContext.Customers.Remove(customer);
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }

    private static ValidationFailure[] GenerateValidationError(string message)
    {
        return new []
        {
            new ValidationFailure(nameof(Customer), message)
        };
    }
}
