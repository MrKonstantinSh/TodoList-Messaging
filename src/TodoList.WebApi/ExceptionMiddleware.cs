using System.Net;
using System.Text.Json;

namespace TodoList.WebApi;

public sealed class ErrorDetails
{
    public string? Title { get; set; }
    public int Status { get; set; }
    
    public override string ToString()
    {
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        
        return JsonSerializer.Serialize(this, serializeOptions);
    }
}

public sealed class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;
    
    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong: {Exception}", ex);
            await HandleExceptionAsync(context);
        }
    }
    
    private static async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsync(new ErrorDetails()
        {
            Title = "Internal Server Error.",
            Status = context.Response.StatusCode
        }.ToString());
    }
}