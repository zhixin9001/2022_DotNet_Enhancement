using Microsoft.EntityFrameworkCore;

namespace _2_EFCore_Basic;

public class PerformanceOptimization
{
    public static void Entry()
    {
        // AsNoTracking();
        // QueryNormally();
        // UpdateFaster();
    }

    public static void AsNoTracking()
    {
        using var ctx = new TestDbContext();
        var book = ctx.Books.AsNoTracking().First();
        book.Title = "new title";
        var entity1 = ctx.Entry(book);
        Console.WriteLine(entity1.State);
    }

    public static void UpdateNormally()
    {
        using var ctx = new TestDbContext();
        var book = ctx.Books.First(); //a=>a.Id==1);
        book.Title = "new title 3";
        ctx.SaveChanges();
    }

    public static void UpdateFaster()
    {
        using var ctx = new TestDbContext();
        var book = new Book() {Id = 1};
        book.Title = "new title 4";
        var entry = ctx.Entry(book);
        entry.Property("Title").IsModified = true;
        Console.WriteLine(entry.DebugView.LongView);
        ctx.SaveChanges();
    }

    public static void Remove()
    {
        using var ctx = new TestDbContext();
        ctx.UpdateRange();
    }
}