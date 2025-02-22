using Microsoft.AspNetCore.Authorization;

namespace API.Source.Guards;

public class Policies
{
    public const string Admin = "Admin";
    public const string Elevated = "Elevated";
    public const string None = "None";
    public const string Moderator = "Moderator";
    public static void AdminPolicyConfig(AuthorizationPolicyBuilder builder) =>
        builder.RequireRole(Admin);

    public static void ElevatedPolicyConfig(AuthorizationPolicyBuilder builder) =>
        builder.RequireRole(Elevated);

    public static void ModeratorPolicyConfig(AuthorizationPolicyBuilder builder) =>
        builder.RequireRole(Moderator);
}