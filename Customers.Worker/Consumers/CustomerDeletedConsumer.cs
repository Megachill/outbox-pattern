using Customers.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Customers.Worker.Consumers;

public class CustomerDeletedConsumer : IConsumer<CustomerDeleted>
{
    private readonly ILogger<CustomerDeletedConsumer> _logger;

    public CustomerDeletedConsumer(ILogger<CustomerDeletedConsumer> logger)
    {
        _logger = logger;
    }
    
    public Task Consume(ConsumeContext<CustomerDeleted> context)
    {
        _logger.LogInformation(context.Message.ToString());
        return Task.CompletedTask;
    }
}
