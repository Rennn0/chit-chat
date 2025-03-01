namespace API.Source.Handlers.Authorization;

public class AaAshit : IRequestHandler<AuthRequest, ResponseModelBase<string>>
{
    public Task<ResponseModelBase<string>> ExecuteAsync(AuthRequest request)
    {
        throw new NotImplementedException();
    }

    public Task ExecuteAsync(PipelineContext<AuthRequest, ResponseModelBase<string>> context)
    {
        throw new NotImplementedException();
    }
}
