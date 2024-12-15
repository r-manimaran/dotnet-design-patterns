using System;

namespace Github.Api.Handlers;

public class LoggingHandler :DelegatingHandler
{
    private readonly ILogger<LoggingHandler> _logger;
    private readonly Random _random = new Random();

    public LoggingHandler(ILogger<LoggingHandler> logger) 
    {
        _logger = logger;
    }
    protected async override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Before Http Request: {method} {url}", request.Method, request.RequestUri);

            if(_random.NextDouble()<0.5)
            {
                _logger.LogWarning("Caught into Random Error for Retry Logic!");
                throw new HttpRequestException("Random error");
            }
            
             var result = await base.SendAsync(request, cancellationToken);
             result.EnsureSuccessStatusCode();
            
             _logger.LogInformation("After Http Request: {method} {url}", request.Method, request.RequestUri);
            
             return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in Http Request: {method} {url}", request.Method, request.RequestUri);
            throw;
        }
    }
}
