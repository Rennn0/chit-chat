using API.Source.Db;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Source.Strategies.ListUsers;

public class ListUsersHandler
    : IRequestHandler<ListUsersRequest, ResponseModelBase<IEnumerable<ApplicationUser>>>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ListUsersHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public Task<ResponseModelBase<IEnumerable<ApplicationUser>>> ExecuteAsync(
        ListUsersRequest request
    )
    {
        throw new NotImplementedException();
    }

    public async Task ExecuteAsync(
        PipelineContext<ListUsersRequest, ResponseModelBase<IEnumerable<ApplicationUser>>> context
    )
    {
        List<ApplicationUser> users = await _userManager.Users.ToListAsync();
        context.Response = new ResponseModelBase<IEnumerable<ApplicationUser>> { Data = users };
    }
}
