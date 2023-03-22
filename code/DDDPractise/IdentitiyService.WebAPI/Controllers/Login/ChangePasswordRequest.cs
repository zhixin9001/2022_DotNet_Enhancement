namespace IdentitiyService.WebAPI.Controllers.Login;

public record ChangePasswordRequest(string Password, string Password2);

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(a => a.Password).NotNull().NotEmpty().Equal(a => a.Password2);
        RuleFor(a => a.Password2).NotNull().NotEmpty();
    }
}