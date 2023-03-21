using IdentityService.Domain.Entities;

namespace IdentitiyService.WebAPI.Controllers.UserAdmin;

public record UserDTO(Guid Id, string UserName, string PhoneNumber, DateTime CreationTime)
{
    public static UserDTO Create(User user)
    {
        return new UserDTO(user.Id, user.UserName, user.PhoneNumber, user.CreationTime);
    }
}