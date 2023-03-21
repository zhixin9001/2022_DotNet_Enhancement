using System.Security.Claims;
using IdentityService.Domain.Entities;
using Microsoft.Extensions.Options;
using ZhiXin.JWT;

namespace IdentityService.Domain;

public class IdDomainService
{
    private readonly IIdRepository _repository;
    private readonly ITokenService _tokenService;
    private readonly IOptionsSnapshot<JWTOptions> _jwtOptions;

    public IdDomainService(IIdRepository repository, ITokenService tokenService,
        IOptionsSnapshot<JWTOptions> jwtOptions)
    {
        _repository = repository;
        _tokenService = tokenService;
        _jwtOptions = jwtOptions;
    }

    public async Task<(SignInResult result, string? token)> LoginAsync(string userName, string pwd)
    {
        var checkResult = await CheckUserNameAndPwdAsync(userName, pwd);
        if (checkResult.Succeeded)
        {
            var user = await _repository.FindByNameAsync(userName);
            string token = await BuildTokenAsync(user!);
            return (SignInResult.Success, token);
        }

        return (checkResult, null);
    }

    private async Task<string> BuildTokenAsync(User user)
    {
        var roles = await _repository.GetRolesAsync(user);
        List<Claim> claims = new();
        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return _tokenService.BuildToken(claims, _jwtOptions.Value);
    }

    private async Task<SignInResult> CheckUserNameAndPwdAsync(string userName, string pwd)
    {
        var user = await _repository.FindByNameAsync(userName);
        if (user == null)
        {
            return SignInResult.Failed;
        }

        var result = await _repository.CheckForSignInAsync(user, pwd, true);
        return result;
    }
}