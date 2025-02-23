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

public class AuthRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string TwoFactorToken { get; set; }
    public AUTH_METHOD Method { get; set; }
    public bool CreateKeyIfNotExists { get; set; } = false;

    public enum AUTH_METHOD
    {
        ApiKey,
        Token,
        Verify2Fa,
    }

    public class GoogleRedirect { }
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
    public TenantConfiguration.TENANT_TYPE Type { get; set; }
    public decimal Price { get; set; }
}

public class ListTenantsRequest
{
    public override int GetHashCode()
    {
        return 2;
    }
}
