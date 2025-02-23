namespace API.Source.Handlers;

public class PipelineContext<TRequest, TResponse>
    where TResponse : new()
{
    public TRequest Request { get; set; }
    public TResponse Response { get; set; }
    public HttpContext? HttpContext { get; set; }

    public PipelineContext(TRequest request, HttpContext? httpContext = null)
    {
        Request = request;
        HttpContext = httpContext;
        Response = new TResponse();
    }
}
