using Microsoft.AspNetCore.Identity;

namespace API.Source.Db;

public class ApplicationUser : IdentityUser
{
    public string ApiKey { get; set; } = string.Empty;
    public DateTime ApiKeyExpiry { get; set; }
}
