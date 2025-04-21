namespace ProductsApi.Decorator;

public abstract class Decorator<TService> where TService : class
{
    protected readonly TService Inner;
    protected Decorator(TService inner)
    {
        Inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }
}
