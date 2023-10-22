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
        config.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.Run();