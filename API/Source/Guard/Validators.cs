using FluentValidation;

namespace API.Source.Guard;

public class AddNewUserRequestValidator : AbstractValidator<AddNewUserRequest>
{
    private static readonly HashSet<string> _allowedRoles = [Policies.Admin, Policies.Elevated];

    public AddNewUserRequestValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
        RuleFor(x => x.Roles).NotEmpty().Must(x => x.All(r => _allowedRoles.Contains(r)));
    }
}

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8);
    }
}
