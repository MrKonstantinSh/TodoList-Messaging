using MassTransit;
using TodoList.EmailSender.Consumers;
using TodoList.EmailSender.EmailSenderService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.AddMassTransit(busConfig =>
{
    busConfig.AddConsumer<EmailSenderConsumer, EmailSenderConsumerDefinition>();
    
    busConfig.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMQ:ConnectionString"]);
        config.UseMessageRetry(r => r.Incremental(5, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30)));
        config.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.Run();