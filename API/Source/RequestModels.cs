#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace API.Source;

public class AddNewUserRequest
{
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public List<string> Roles { get; set; } = [];
}

public class LoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class ListUsersRequest
{
    public override int GetHashCode()
    {
        return 1;
    }
}
