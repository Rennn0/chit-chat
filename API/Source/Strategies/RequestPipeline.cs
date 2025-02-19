namespace API.Source.Strategies;

public class RequestPipeline<TRequest, TResponse> : IRequestPipeline<TRequest, TResponse>
    where TResponse : new()
{
    private readonly IEnumerable<IRequestHandler<TRequest, TResponse>> _handlers;

    public RequestPipeline(IEnumerable<IRequestHandler<TRequest, TResponse>> handlers)
    {
        _handlers = handlers;
    }

    public async Task<TResponse> ExecuteAsync(TRequest request)
    {
        PipelineContext<TRequest, TResponse> context = new PipelineContext<TRequest, TResponse>(
            request
        );

        foreach (IRequestHandler<TRequest, TResponse> handler in _handlers)
        {
            await handler.ExecuteAsync(context);
        }

        return context.Response ?? new TResponse();
    }
}
