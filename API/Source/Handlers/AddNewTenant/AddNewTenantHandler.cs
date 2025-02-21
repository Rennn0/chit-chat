using API.Source.Db.Models;
using API.Source.Db.Repo;

namespace API.Source.Handlers.AddNewTenant;

public class AddNewTenantHandler
    : IRequestHandler<AddNewTenantRequest, ResponseModelBase<AddNewTenantResponse>>
{
    private readonly IUnitOfWork _work;

    public AddNewTenantHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public Task<ResponseModelBase<AddNewTenantResponse>> ExecuteAsync(AddNewTenantRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task ExecuteAsync(
        PipelineContext<AddNewTenantRequest, ResponseModelBase<AddNewTenantResponse>> context
    )
    {
        await _work.BeginTransactionAsync();
        context.Response = new ResponseModelBase<AddNewTenantResponse>
        {
            Data = new AddNewTenantResponse
            {
                Configuration = new TenantConfiguration()
                {
                    Price = context.Request.Price,
                    Type = context.Request.Type,
                },
            },
        };
        await _work
            .GetRepository<TenantConfiguration>()
            .AddAsync(context.Response.Data.Configuration);

        await _work.CommitAsync();
    }
}
