namespace API.Source.Strategies;

public interface IRequestPipeline<in TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request);
}
