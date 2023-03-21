using IdentityService.Infrastructure;
using IdentityService.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentitiyService.WebAPI.Controllers.UserAdmin;

[Route("[controller]/[action]")]
[ApiController]
// [Authorize(Roles = "Admin")]
public class UserAdminController : ControllerBase
{
    private readonly IdUserManager _userManager;
    private readonly IIdRepository _repository;

    public UserAdminController(IdUserManager userManager, IIdRepository repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    [HttpGet]
    public Task<UserDTO[]> FindAllUsers()
    {
        return _userManager.Users.Select(u => UserDTO.Create(u)).ToArrayAsync();
    }
}