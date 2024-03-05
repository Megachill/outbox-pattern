using Customers.Api;
using Customers.Api.Data;
using Customers.Api.Services;
using Customers.Api.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

var config = builder.Configuration;
config.AddEnvironmentVariables("CustomersApi_");

builder.Services.AddControllers();
    
builder.Services.AddFluentValidationAutoValidation(c =>
{
    c.DisableDataAnnotationsValidation = true;
});
builder.Services.AddValidatorsFromAssemblyContaining<IApiMarker>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(x =>
{
    x.AddEntityFrameworkOutbox<AppDbContext>(o =>
    {
        o.QueryDelay = TimeSpan.FromSeconds(5);
        //o.UsePostgres().UseBusOutbox();
        o.UsePostgres().DisableInboxCleanupService();
        o.UseBusOutbox(o =>
        {
            //o.DisableDeliveryService();
            o.MessageDeliveryLimit = 1;
        });
    });
    
    /*x.UsingAmazonSqs((ctx, cfg) =>
    {
        cfg.Host("eu-west-2", _ => {});
        cfg.ConfigureEndpoints(ctx);
    });*/
    
    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(config["RabbitMQ:Host"], "/", h =>
        {
            h.Username("rabbitmq");
            h.Password("rabbitmq");
        });
        
        cfg.ConfigureEndpoints(ctx);
        cfg.AutoStart = true;
    });
});

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseNpgsql(config["Database:ConnectionString"]!, opt =>
    {
        opt.EnableRetryOnFailure(5);
    });
});
builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ValidationExceptionMiddleware>();
app.MapControllers();

app.Run();
