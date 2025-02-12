using Microsoft.AspNetCore.Authorization;

namespace API.Source.Guard;

public class Policies
{
    public const string Admin = "Admin";
    public const string Elevated = "Elevated";

    public static void AdminPolicyConfig(AuthorizationPolicyBuilder builder) =>
        builder.RequireRole(Admin);

    public static void ElevatedPolicyConfig(AuthorizationPolicyBuilder builder) =>
        builder.RequireRole(Elevated);
}
