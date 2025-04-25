using System.Reflection;

namespace ProductsApi.Decorator;

public class LoggingDecorator<T> : DispatchProxy where T : class
{
    private T _decorated;
    private ILogger<T> _logger;

    public static T Create(T decorated, ILogger<T> logger)
    {
        object proxy = Create<T, LoggingDecorator<T>>();
        ((LoggingDecorator<T>)proxy).SetParameters(decorated, logger);
        return (T)proxy;
    }
    private void SetParameters(T decorated, ILogger<T> logger)
    {
        _decorated = decorated;
        _logger = logger;
    }
    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        _logger.LogInformation("Logger called: Calling method {Method} at {Date}", targetMethod.Name, DateTime.Now);
        var result = targetMethod.Invoke(_decorated, args);
        _logger.LogInformation("Logger called: Finished method {Method} at {Date}", targetMethod.Name, DateTime.Now);
        return result;
    }
}
