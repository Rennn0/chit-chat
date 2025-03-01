namespace API.Source.Handlers;

public class RequestPipeline<TRequest, TResponse> : IRequestPipeline<TRequest, TResponse>
    where TResponse : new()
{
    private readonly IEnumerable<IRequestHandler<TRequest, TResponse>> _handlers;

    public RequestPipeline(IEnumerable<IRequestHandler<TRequest, TResponse>> handlers)
    {
        _handlers = handlers;
    }

    public async Task<TResponse> ExecuteAsync(TRequest request, HttpContext? httpContext = null)
    {
        PipelineContext<TRequest, TResponse> context = new PipelineContext<TRequest, TResponse>(
            request,
            httpContext
        );

        foreach (IRequestHandler<TRequest, TResponse> handler in _handlers)
        {
            try
            {
                await handler.ExecuteAsync(context);
            }
            catch (Exception e)
            {
                context.HasError = true;
                context.AggregatedErrors.AddLast(
                    new PipelineContext<TRequest, TResponse>.PipelineAggregatedError()
                    {
                        Message = $"Handler {handler.GetType().Name} failed: {e.Message}",
                        StackTrace = e.StackTrace,
                    }
                );
            }
        }

        // TODO logging

        return context.Response ?? new TResponse();
    }
}
