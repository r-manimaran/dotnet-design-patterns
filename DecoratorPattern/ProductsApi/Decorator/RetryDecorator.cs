using Polly;

namespace ProductsApi.Decorator;

public class RetryDecorator<TService>: Decorator<TService> where TService: class
{
    private readonly IAsyncPolicy _retryPolicy;

    public RetryDecorator(TService inner, IAsyncPolicy retryPolicy)
        : base(inner)
    {
        _retryPolicy = retryPolicy ?? throw new ArgumentNullException(nameof(retryPolicy));
    }

    public async Task<TResult> ExecuteAsync<TResult>(Func<TService, Task<TResult>> operation)
    {
        return await _retryPolicy.ExecuteAsync(async () => await operation(Inner));
    }
}
