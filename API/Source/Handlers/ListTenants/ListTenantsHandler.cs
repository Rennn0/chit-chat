using API.Source.Db.Models;
using API.Source.Db.Repo;

namespace API.Source.Handlers.ListTenants;

public class ListTenantsHandler
    : IRequestHandler<ListTenantsRequest, ResponseModelBase<IEnumerable<ListTenantsResponse>>>
{
    private readonly IUnitOfWork _work;

    public ListTenantsHandler(IUnitOfWork work)
    {
        _work = work;
    }

    public Task<ResponseModelBase<IEnumerable<ListTenantsResponse>>> ExecuteAsync(
        ListTenantsRequest request
    )
    {
        throw new NotImplementedException();
    }

    public async Task ExecuteAsync(
        PipelineContext<
            ListTenantsRequest,
            ResponseModelBase<IEnumerable<ListTenantsResponse>>
        > context
    )
    {
        IEnumerable<TenantConfiguration?> tenantConfigs = await _work
            .GetRepository<TenantConfiguration>()
            .GetAllAsync();
        context.Response.Data = tenantConfigs.Select(tc =>
            tc is null
                ? null
                : new ListTenantsResponse()
                {
                    Type = tc.Type,
                    Id = tc.Id,
                    Price = tc.Price,
                }
        );
    }
}
