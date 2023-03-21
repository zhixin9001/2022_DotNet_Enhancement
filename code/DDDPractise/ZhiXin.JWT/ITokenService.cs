using System.Security.Claims;

namespace ZhiXin.JWT;

public interface ITokenService
{
    string BuildToken(IEnumerable<Claim> claims, JWTOptions options);
}