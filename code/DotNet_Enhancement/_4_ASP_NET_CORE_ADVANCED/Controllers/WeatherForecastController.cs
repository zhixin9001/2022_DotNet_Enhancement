using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
           var jwt= BuildToken(dbUser.UserName);
            return Ok(jwt);
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

    string key = "123qwe123456789qwertyui123qwe123456789qwertyui123qwe123456789qwertyui123qwe123456789qwertyui123qwe123456789qwertyui";
    
    [HttpGet("jwt")]
    public Task<string> JWT()
    {
        var jwt = BuildToken("zx");
        return Task.FromResult(jwt);
    }

    private string BuildToken(string name)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, "6"));
        claims.Add(new Claim(ClaimTypes.Name, name));
        claims.Add(new Claim(ClaimTypes.Role, "user"));
        claims.Add(new Claim(ClaimTypes.Role, "admin"));

        var expire = DateTime.Today.AddDays(1);
        var secBytes = Encoding.UTF8.GetBytes(key);
        var secKey = new SymmetricSecurityKey(secBytes);
        var credential = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(claims: claims, expires: expire, signingCredentials: credential);
        string jwt = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        return jwt;
    }

    [HttpGet("jwt-validate")]
    public Task<string> JWT_Validate(string jwt)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        TokenValidationParameters valParam = new();
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        valParam.IssuerSigningKey = securityKey;
        valParam.ValidateIssuer = false;
        valParam.ValidateAudience = false;
        ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(jwt, valParam, out SecurityToken securityToken);

        StringBuilder builder = new();
        foreach (var claim in claimsPrincipal.Claims)
        {
            builder.Append($"{claim.Type}={claim.Value}");
        }

        return Task.FromResult(builder.ToString());
    }
}