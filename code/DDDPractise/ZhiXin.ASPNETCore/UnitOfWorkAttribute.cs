using Microsoft.EntityFrameworkCore;

namespace ZhiXin.ASPNETCore;

public class UnitOfWorkAttribute : Attribute
{
    public Type[] DbContextTypes { get; init; }

    public UnitOfWorkAttribute(params Type[] dbContextTypes)
    {
        this.DbContextTypes = dbContextTypes;

        foreach (var type in dbContextTypes)
        {
            if (!typeof(DbContext).IsAssignableFrom(type))
            {
                throw new ArgumentException($"{type} must inhert from DbContext");
            }
        }
    }
}