using MassTransit;
using ServiceA.Consumers;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHttpClient();
builder.Services.AddMassTransit(configurator =>
{
    configurator.AddConsumer<ServiceBSentMessageConsumer>();
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
        _configure.ReceiveEndpoint("servicea-message-queue", e => e.ConfigureConsumer<ServiceBSentMessageConsumer>(context));
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapGet("/", async (HttpClient httpClient) =>
{
    var response = await httpClient.GetAsync("https://localhost:7257/api/data");
    response.EnsureSuccessStatusCode();
    var data = await response.Content.ReadAsStringAsync();
    return Results.Ok(data);
});

app.Run();