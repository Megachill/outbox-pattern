using Customers.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Customers.Worker.Consumers;

public class CustomerCreatedConsumer : IConsumer<CustomerCreated>
{
    private readonly ILogger<CustomerCreatedConsumer> _logger;

    public CustomerCreatedConsumer(ILogger<CustomerCreatedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CustomerCreated> context)
    {
        _logger.LogInformation(context.Message.ToString());
        return Task.CompletedTask;
    }
}
