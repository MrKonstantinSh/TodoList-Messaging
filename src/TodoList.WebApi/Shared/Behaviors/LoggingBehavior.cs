using System.Diagnostics;
using MediatR;

namespace TodoList.WebApi.Shared.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly Stopwatch _stopwatch;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;

        _stopwatch = new Stopwatch();
    }


    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        _stopwatch.Start();

        var response = await next();
        
        _stopwatch.Stop();
        
        var requestName = typeof(TRequest).Name;
        _logger.LogInformation("TodoList.WebApi Request: {Name} takes {ElapsedMilliseconds}ms",
            requestName, _stopwatch.ElapsedMilliseconds);

        return response;
    }
}