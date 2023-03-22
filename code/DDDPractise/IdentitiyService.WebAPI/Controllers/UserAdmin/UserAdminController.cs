using IdentitiyService.WebAPI.Events;
using MediatR;

namespace IdentitiyService.WebAPI.Controllers.UserAdmin;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Roles = "Admin")]
public class UserAdminController : ControllerBase
{
    private readonly IdUserManager _userManager;
    private readonly IIdRepository _repository;
    private readonly IMediator _mediator;

    public UserAdminController(IdUserManager userManager, IIdRepository repository, IMediator mediator)
    {
        _userManager = userManager;
        _repository = repository;
        _mediator = mediator;
    }

    [HttpGet]
    public Task<UserDTO[]> FindAllUsers()
    {
        return _userManager.Users.Select(u => UserDTO.Create(u)).ToArrayAsync();
    }

    [HttpPost]
    public async Task<ActionResult> AddAdminUser(AddAdminUserRequest req)
    {
        (var result, var user, var password) = await _repository.AddAdminUserAsync(req.UserName, req.PhoneNumber);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.SumErrors());
        }

        await _mediator.Send(new UserCreatedEvent(user.Id, user.UserName, password, user.PhoneNumber));
        return Ok();
    }
}