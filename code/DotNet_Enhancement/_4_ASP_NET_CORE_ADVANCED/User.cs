using Microsoft.AspNetCore.Identity;

namespace _4_ASP_NET_CORE_ADVANCED;

public class User: IdentityUser<long>
{
    public DateTime CreationTime { get; set; }
    public string NickName { get; set; } = string.Empty;
}

public class Role : IdentityRole<long>
{
    
}