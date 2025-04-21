namespace ProductsApi.Decorator;

public class LoggingDecorator<TService> : Decorator<TService> where TService : class
{
    private readonly ILogger<LoggingDecorator<TService>> _logger;
    public LoggingDecorator(TService inner, ILogger<LoggingDecorator<TService>> logger) : base(inner)
    {

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    // Use dynamic dispatch to forward calls to the inner service
    public TResult Execute<TResult>(Func<TService, TResult> operation)
    {
        try
        {
            _logger.LogInformation($"Executing {operation.Method.Name}");
            var result = operation(Inner);
            _logger.LogInformation($"Completed {operation.Method.Name}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error executing {operation.Method.Name}");
            throw;

        }
    }
}
