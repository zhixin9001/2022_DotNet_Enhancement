using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _4_ASP_NET_CORE_ADVANCED.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class Auth : ControllerBase
{
    [HttpGet("hello")]
    public IActionResult Index()
    {
        return Ok("hello");
    }
}