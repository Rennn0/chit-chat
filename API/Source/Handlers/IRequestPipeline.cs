namespace API.Source.Handlers;

public interface IRequestPipeline<in TRequest, TResponse>
{
    Task<TResponse> ExecuteAsync(TRequest request, HttpContext? httpContext = null);
}
