namespace IdentitiyService.WebAPI.Controllers.UserAdmin;

public record AddAdminUserRequest(string UserName, string PhoneNumber);

public class AddAdminUserRequestValidator : AbstractValidator<AddAdminUserRequest>
{
    public AddAdminUserRequestValidator()
    {
        RuleFor(a => a.UserName).NotNull().NotEmpty().MaximumLength(20).MinimumLength(5);
        RuleFor(a => a.PhoneNumber).NotNull().NotEmpty().Length(11);
    }
}