using FluentValidation;

namespace API.Source.Guards;

public class AddNewUserRequestValidator : AbstractValidator<AddNewUserRequest>
{
    private static readonly HashSet<string> _allowedRoles =
    [
        Policies.Elevated,
        Policies.Moderator,
        Policies.None
    ];

    public AddNewUserRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Roles).NotEmpty().Must(x => x.All(r => _allowedRoles.Contains(r)));
    }
}

public class LoginRequestValidator : AbstractValidator<AuthRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}