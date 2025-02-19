using API.Source.Strategies;

namespace API.Source.Factory;

public interface IRequestHandlerFactory
{
    IRequestStrategy<TRequest, TResponse> GetStrategy<TRequest, TResponse>()
        where TResponse : new();

    IRequestPipeline<TRequest, TResponse> GetPipeline<TRequest, TResponse>();
}

public class RequestHandlerFactory : IRequestHandlerFactory
{
    private readonly IServiceProvider _provider;

    public RequestHandlerFactory(IServiceProvider provider)
    {
        _provider = provider;
    }

    public IRequestStrategy<TRequest, TResponse> GetStrategy<TRequest, TResponse>()
        where TResponse : new()
    {
        return _provider.GetRequiredService<IRequestStrategy<TRequest, TResponse>>();
    }

    public IRequestPipeline<TRequest, TResponse> GetPipeline<TRequest, TResponse>()
    {
        return _provider.GetRequiredService<IRequestPipeline<TRequest, TResponse>>();
    }
}
