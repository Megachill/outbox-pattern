using Customers.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Customers.Worker.Consumers;

public class CustomerUpdatedConsumer : IConsumer<CustomerUpdated>
{
    private readonly ILogger<CustomerUpdatedConsumer> _logger;

    public CustomerUpdatedConsumer(ILogger<CustomerUpdatedConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<CustomerUpdated> context)
    {
        _logger.LogInformation(context.Message.ToString());
        return Task.CompletedTask;
    }
}
