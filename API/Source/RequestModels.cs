using API.Source.Db.Models;

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
    public string Username { get; set; }
    public string Password { get; set; }
    public AuthMethod Method { get; set; }
    public bool CreateKeyIfNotExists { get; set; } = false;
    public enum AuthMethod
    {
        ApiKey,
        Token
    }
}

public class ListUsersRequest
{
    public override int GetHashCode()
    {
        return 1;
    }
}

public class AddNewTenantRequest
{
    public TenantConfiguration.TenantType Type { get; set; }
    public decimal Price { get; set; }
    public bool Throw { get; set; } = false;
}

public class ListTenantsRequest
{
    public override int GetHashCode()
    {
        return 2;
    }
}
