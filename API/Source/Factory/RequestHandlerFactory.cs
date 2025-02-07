using API.Source.Strategy;

namespace API.Source.Factory;

public interface IRequestHandlerFactory
{
    IRequestHandlerStrategy<TRequest, TStrategy> GetHandler<TRequest, TStrategy>();
}

public class RequestHandlerFactory : IRequestHandlerFactory
{
    private readonly IServiceProvider _provider;

    public RequestHandlerFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IRequestHandlerStrategy<TRequest, TStrategy> GetHandler<TRequest, TStrategy>()
    {
        return _provider.GetRequiredService<IRequestHandlerStrategy<TRequest, TStrategy>>();
    }
}
