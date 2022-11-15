namespace _2_EFCore_Basic;

public class DataReaderDataSet
{
    public static void Entry()
    {
        // var books = QueryableBooks();
        // books.First();
        NestQuery();
    }

    public static void NestQuery()
    {
        using var ctx = new TestDbContext();
        var books = ctx.Books.Where(a => a.Id > 0);
        foreach (var book in books)
        {
            foreach (var article in ctx.Article)
            {
                
            }
        }
    }
    
    public static IQueryable<Book> QueryableBooks()
    {
        using var ctx = new TestDbContext();
        return ctx.Books.Where(a => a.Id >= 0);
    }
}