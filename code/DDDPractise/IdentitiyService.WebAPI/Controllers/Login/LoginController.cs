using System.Security.Claims;

namespace IdentitiyService.WebAPI.Controllers.Login;

[Route("[controller]/[action]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IdDomainService _idDomainService;
    private readonly IIdRepository _repository;

    public LoginController(IdDomainService idDomainService, IIdRepository repository)
    {
        _idDomainService = idDomainService;
        _repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult> CreateWorld()
    {
        if (await _repository.FindByNameAsync("admin") != null)
        {
            return Conflict("Already initialized");
        }

        User user = new("admin");
        var r = await _repository.CreateAsync(user, "123456");
        Debug.Assert(r.Succeeded);
        var token = await _repository.GenerateChangePhoneNumberTokenAsync(user, "18165151111");
        var cr = await _repository.ChangePhoneNumAsync(user.Id, "18165151111", token);
        Debug.Assert(cr.Succeeded);
        r = await _repository.AddToRoleAsync(user, "User");
        Debug.Assert(r.Succeeded);
        r = await _repository.AddToRoleAsync(user, "Admin");
        Debug.Assert(r.Succeeded);
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<string>> Login(string userName, string password)
    {
        (var checkResult, var token) = await _idDomainService.LoginAsync(userName, password);
        if (checkResult.Succeeded)
        {
            return token;
        }
        else if (checkResult.IsLockedOut)
        {
            return StatusCode((int) HttpStatusCode.Locked, "Failed too many times");
        }
        else
        {
            var msg = checkResult.ToString();
            return BadRequest("Login failed:" + msg);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult> ChangePassword(ChangePasswordRequest req)
    {
        var userId = Guid.Parse(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
        var result = await _repository.ChangePasswordAsync(userId, req.Password);
        if (result.Succeeded)
        {
            return Ok();
        }
        else
        {
            return BadRequest(result.Errors.SumErrors());
        }
    }
}