namespace API.Source.Strategies;

public class PipelineContext<TRequest, TResponse>
    where TResponse : new()
{
    public TRequest Request { get; set; }
    public TResponse Response { get; set; }

    public PipelineContext(TRequest request)
    {
        Request = request;
        Response = new TResponse();
    }
}
