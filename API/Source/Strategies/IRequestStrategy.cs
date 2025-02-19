namespace API.Source.Strategies;

public interface IRequestStrategy<TRequest, TResponse>
    where TResponse : new()
{
    Task<TResponse> ExecuteAsync(TRequest request);
    Task ExecuteAsync(PipelineContext<TRequest, TResponse> context);
}
