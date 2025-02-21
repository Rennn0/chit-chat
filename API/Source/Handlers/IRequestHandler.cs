namespace API.Source.Handlers;

public interface IRequestHandler<TRequest, TResponse>
    where TResponse : new()
{
    /// <summary>
    ///    this is the method that will be called by the controller, without context
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<TResponse> ExecuteAsync(TRequest request);

    /// <summary>
    ///     this is the method that will be called by the pipeline and contain context
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task ExecuteAsync(PipelineContext<TRequest, TResponse> context);
}
