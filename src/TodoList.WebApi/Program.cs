using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TodoList.WebApi;
using TodoList.WebApi.DataAccess;
using TodoList.WebApi.Features.Users;
using TodoList.WebApi.Features.Users.Commands;
using TodoList.WebApi.Features.Users.Validators;
using TodoList.WebApi.Shared.Behaviors;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(UserMappingProfile));
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserValidator>();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblyContaining<CreateUserCommand>();
    
    config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    config.RegisterValidationBehaviors();
});

builder.Services.AddMassTransit(busConfig =>
{
    busConfig.UsingRabbitMq((_, config) =>
    {
        config.Host(builder.Configuration["RabbitMQ:ConnectionString"]);
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Todo List - API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var logger = app.Services.GetRequiredService<ILogger<ExceptionMiddleware>>();
app.UseMiddleware<ExceptionMiddleware>(logger);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();