// See https://aka.ms/new-console-template for more information

using _2_EFCore_Basic;
using Microsoft.EntityFrameworkCore;

await Locks.Entry();
// ReadWriteArticleComment.Entry();
// StateTracking.Entry();
// PerformanceOptimization.Entry();
// Books
using var ctx = new TestDbContext();
// Save(ctx);
// Update(ctx);
// Query(ctx);
// PageOutput(0);
// PageOutput(1);
// PageOutput(2);
// DataReaderDataSet.Entry();
// QueryWithSql(ctx);

void QueryWithSql(TestDbContext ctx)
{
    var id = 0;
    // var b= ctx.Database.ExecuteSqlInterpolated($"select count(*) from T_Books where id>{id}");
    var b= ctx.Books.FromSqlInterpolated($"select * from T_Books where id>{id}");
    Console.WriteLine(b.First().Title);
}

void Query(TestDbContext ctx)
{
    var book = ctx.Books.FirstOrDefault(a => a.Price > 10);
    Console.WriteLine(book.Title);
}

void Update(TestDbContext ctx)
{
    var book = ctx.Books.FirstOrDefault();
    book.Title = "new title";
    ctx.SaveChanges();
}

void Save(TestDbContext ctx)
{
    for (int i = 0; i < 33; i++)
    {
        var b1 = new Book
        {
            AuthorName = "a",
            Price = 12,
            PubTime = DateTime.Now,
            Title = $"title {i}"
        };

        ctx.Books.Add(b1);
    }

    ctx.SaveChanges();
}

void PageOutput(int pageIndex, int pageSize = 2)
{
    var books = ctx.Books.Where(a => a.Price > 10).OrderBy(a => a.Id);
    var count = books.LongCount();
    var pageCount = (long) Math.Ceiling((double) count / pageSize);
    Console.WriteLine($"Page Count: {pageCount}");
    var pagedBooks = books.Skip(pageIndex * pageSize).Take(pageSize);
    foreach (var pagedBook in pagedBooks)
    {
        Console.WriteLine($"{pagedBook.Id}, {pagedBook.Title}");
    }
}