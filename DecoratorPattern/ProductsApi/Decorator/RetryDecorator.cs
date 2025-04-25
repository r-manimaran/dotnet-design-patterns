using Polly;
using System.Reflection;

namespace ProductsApi.Decorator;

public class RetryDecorator<TService> : DispatchProxy where TService : class
{
    private TService _decorated;
    private AsyncPolicy _retryPolicy;

    public static TService Create(TService decorated, AsyncPolicy retryPolicy)
    {
        var proxy = Create<TService, RetryDecorator<TService>>();
        ((RetryDecorator<TService>)(object)proxy).SetParameters(decorated, retryPolicy);
        return proxy;
    }

    private void SetParameters(TService decorated, AsyncPolicy retryPolicy)
    {
        _decorated = decorated;
        _retryPolicy = retryPolicy;
    }

    // protected override object Invoke(MethodInfo targetMethod, object[] args)
    // {
    //     //if (typeof(Task).IsAssignableFrom(targetMethod.ReturnType))
    //     //{
    //         return _retryPolicy.ExecuteAsync(() =>
    //             (Task)targetMethod.Invoke(_decorated, args));
    //     //}

    //     // return _retryPolicy.Execute(() =>
    //     //     targetMethod.Invoke(_decorated, args));
    // }

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
        // Handle async methods
        if (typeof(Task).IsAssignableFrom(targetMethod.ReturnType))
        {
            // Handle Task<T> (methods that return a value)
            if (targetMethod.ReturnType.IsGenericType)
            {
                var resultType = targetMethod.ReturnType.GetGenericArguments()[0];
                return typeof(RetryDecorator<TService>)
                    .GetMethod(nameof(InvokeAsyncWithResult), BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.MakeGenericMethod(resultType)
                    .Invoke(this, new object[] { targetMethod, args });
            }
            // Handle Task (void returning methods)
            else
            {
                return InvokeAsync(targetMethod, args);
            }
        }

        // Handle synchronous methods
        return targetMethod.Invoke(_decorated, args);
    }

    private async Task<T> InvokeAsyncWithResult<T>(MethodInfo targetMethod, object[] args)
    {
        return await _retryPolicy.ExecuteAsync(async () =>
        {
            var task = (Task<T>)targetMethod.Invoke(_decorated, args);
            return await task;
        });
    }

    private async Task InvokeAsync(MethodInfo targetMethod, object[] args)
    {
        await _retryPolicy.ExecuteAsync(async () =>
        {
            var task = (Task)targetMethod.Invoke(_decorated, args);
            await task;
        });
    }
}