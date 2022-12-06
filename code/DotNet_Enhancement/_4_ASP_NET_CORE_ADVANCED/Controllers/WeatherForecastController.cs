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

    public WeatherForecastController(ILogger<WeatherForecastController> logger, RoleManager<Role> roleManager, UserManager<User> userManager)
    {
        _logger = logger;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }    
    
    [HttpGet(Name = "auth")]
    public async Task<IActionResult> Auth()
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
        if (user==null)
        {
            user = new User() {UserName = "zx", Email = "zhixin9001@126.com", EmailConfirmed = false};
            var result = await _userManager.CreateAsync(user);
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
}