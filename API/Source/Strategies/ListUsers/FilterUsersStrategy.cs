using API.Source.Db;

namespace API.Source.Strategies.ListUsers;

public class FilterUsersStrategy
    : IRequestStrategy<ListUsersRequest, ResponseModelBase<IEnumerable<ApplicationUser>>>
{
    public Task<ResponseModelBase<IEnumerable<ApplicationUser>>> ExecuteAsync(
        ListUsersRequest request
    )
    {
        throw new NotImplementedException();
    }

    public Task ExecuteAsync(
        PipelineContext<ListUsersRequest, ResponseModelBase<IEnumerable<ApplicationUser>>> context
    )
    {
        if (context.Response.Data != null)
            context.Response.Data = context.Response.Data.Where(u => u.EmailConfirmed).ToList();

        return Task.CompletedTask;
    }
}
