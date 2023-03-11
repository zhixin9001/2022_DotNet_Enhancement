namespace _4_ASP_NET_CORE_ADVANCED;

public class JWTOption
{
    public string SigningKey { get; set; }
    public int ExpireSeconds { get; set; }
}