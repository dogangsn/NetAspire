using MassTransit;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/send-message", async (ISendEndpointProvider sendEndpointProvider) =>
{
    var sendEndpoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:servicea-message-queue"));
    await sendEndpoint.Send(new Message { Text = $"Message sent : {DateTime.Now}" });
});

app.MapGet("/api/data", () => "Hello World! : ServiceB");

app.Run();