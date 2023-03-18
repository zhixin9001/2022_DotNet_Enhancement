using System.Security.Cryptography;
using System.Text;

namespace Zhixin.Commons;

public static class HashHelper
{
    private static string ToHashString(byte[] bytes)
    {
        StringBuilder builder = new();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }

        return builder.ToString();
    }

    public static string ComputeSha256Hash(Stream stream)
    {
        using SHA256 sha256=SHA256.Create();
        byte[] bytes = sha256.ComputeHash(stream);
        return ToHashString(bytes);
    }

    public static string ComputeSha256Hash(string input)
    {
        using SHA256 sha256=SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
        return ToHashString(bytes);
    }

    public static string ComputeMd5Hash(Stream stream)
    {
        using MD5 md5=MD5.Create();
        byte[] bytes = md5.ComputeHash(stream);
        return ToHashString(bytes);
    }

    public static string ComputeMd5Hash(string input)
    {
        using MD5 md5=MD5.Create();
        byte[] bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
        return ToHashString(bytes);
    }
    
    
}