using Microsoft.AspNetCore.Identity;

namespace API.Source.Db;

public class ApplicationUser : IdentityUser
{
    public string ApiKey { get; set; }
    public DateTime ApiKeyExpiry { get; set; }
}
