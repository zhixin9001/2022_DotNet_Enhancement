using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _4_ASP_NET_CORE_ADVANCED.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly RoleManager<Role> _roleManager;
    private readonly UserManager<User> _userManager;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, RoleManager<Role> roleManager,
        UserManager<User> userManager)
    {
        _logger = logger;
        _roleManager = roleManager;
        _userManager = userManager;
    }
    //
    // [HttpGet(Name = "GetWeatherForecast")]
    // public IEnumerable<WeatherForecast> Get()
    // {
    //     return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    //         {
    //             Date = DateTime.Now.AddDays(index),
    //             TemperatureC = Random.Shared.Next(-20, 55),
    //             Summary = Summaries[Random.Shared.Next(Summaries.Length)]
    //         })
    //         .ToArray();
    // }

    [HttpGet("init")]
    public async Task<IActionResult> Init()
    {
        var roleExists = await _roleManager.RoleExistsAsync("admin");
        if (!roleExists)
        {
            var role = new Role() {Name = "Admin"};
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
        }

        var user = await _userManager.FindByNameAsync("zx");
        if (user == null)
        {
            user = new User() {UserName = "zx", Email = "zhixin9001@126.com", EmailConfirmed = true};
            var result = await _userManager.CreateAsync(user, "123456");
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var resultSetUserRole = await _userManager.AddToRoleAsync(user, "Admin");
            if (!resultSetUserRole.Succeeded)
            {
                return BadRequest(resultSetUserRole.Errors);
            }
        }

        return Ok();
    }
    
    [HttpGet("login/{user}/{pwd}")]
    public async Task<IActionResult> Login([FromRoute] string user, [FromRoute] string pwd)
    {
        var dbUser = await _userManager.FindByNameAsync(user);
        if (dbUser == null)
        {
            return NotFound();
        }
    
        if (await _userManager.IsLockedOutAsync(dbUser))
        {
            return BadRequest("Locked");
        }
    
        var isLoginSucceed = await _userManager.CheckPasswordAsync(dbUser, pwd);
        if (isLoginSucceed)
        {
            return Ok("Login succeed");
        }
        else
        {
            await _userManager.AccessFailedAsync(dbUser);
            return BadRequest("Login failed");
        }
    }

     [HttpGet("reset")]
     public async Task<IActionResult> ResetPwd(string email)
     {
         var user = await _userManager.FindByEmailAsync(email);
         if (user == null)
         {
             return NotFound();
         }

         var token = await _userManager.GeneratePasswordResetTokenAsync(user);
         _logger.LogInformation($"token is {token}");
         return Ok($"{token}");
     }

     [HttpGet("verify-reset/{email}/{pwd}/{token}")]
     public async Task<IActionResult> VerifyReset([FromRoute] string email, [FromRoute] string pwd,
         [FromRoute] string token)
     {
         var user = await _userManager.FindByEmailAsync(email);
         if (user == null)
         {
             return NotFound();
         }

         var result = await _userManager.ResetPasswordAsync(user, token, pwd);
         if (result.Succeeded)
         {
             return Ok();
         }
         return BadRequest();
     }
}