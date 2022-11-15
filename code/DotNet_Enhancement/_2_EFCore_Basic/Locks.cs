using Microsoft.EntityFrameworkCore;

namespace _2_EFCore_Basic;

public class Locks
{
    public static async Task Entry()
    {
        Console.WriteLine("press any key");
        Console.ReadLine();
        await PessimisticLock();
    }

    public static async Task PessimisticLock()
    {
        using var ctx = new TestDbContext();
        using var tx = await ctx.Database.BeginTransactionAsync();
        Console.WriteLine("selecting "+DateTime.Now.TimeOfDay);
        var b1 = await ctx.Books.FromSqlInterpolated($"select * from T_Books where id=34 for update").SingleAsync();
        if (b1.Price == 0)
        {
            await Task.Delay(5000);
            b1.Price = 999;
            await ctx.SaveChangesAsync();
            Console.WriteLine("succeed");
        }
        else
        {
            Console.WriteLine("failed");
        }

        await tx.CommitAsync();
    }
}