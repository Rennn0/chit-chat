namespace API.Source.Handlers;

public class PipelineContext<TRequest, TResponse>
    where TResponse : new()
{
    public class PipelineAggregatedError
    {
        public string? StackTrace { get; set; }
        public string Message { get; set; }
    }

    public TRequest Request { get; set; }
    public TResponse Response { get; set; }
    public HttpContext? HttpContext { get; set; }
    public LinkedList<PipelineAggregatedError> AggregatedErrors { get; set; }
    public bool HasError { get; set; }

    public PipelineContext(TRequest request, HttpContext? httpContext = null)
    {
        Request = request;
        HttpContext = httpContext;
        Response = new TResponse();
        AggregatedErrors = [];
        HasError = false;
    }
}
