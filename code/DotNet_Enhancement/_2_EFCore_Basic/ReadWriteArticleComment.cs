using Microsoft.EntityFrameworkCore;

namespace _2_EFCore_Basic;

public class ReadWriteArticleComment
{
    public static void Entry()
    {
        // Save();
        Query();
    }

    static void Query()
    {
        using var ctx = new TestDbContext();
        // var article1 = ctx.Article.Include(a => a.Comments).Single(a => a.Id == 2);
        // IQueryable<Article> articles = ctx.Article.Where(a=>a.Id==2);
        // Console.WriteLine(articles.First());
        // Console.WriteLine(article1.Comments.First().ArticleId);

        var articles = ctx.Article.Where(a => a.Id > 0);
        var articles1 = articles.Where(a => !string.IsNullOrEmpty(a.Title));
        var articles1Ordered = articles1.OrderBy(a => a.Id);
        
        Console.WriteLine(articles1Ordered.Last());
    }

    public static void Save()
    {
        using var ctx = new TestDbContext();
        var article1 = new Article
        {
            Title = "article 2",
        };
        article1.Comments.Add(new Comment()
        {
            Message = "message1 for article 2",
        });
        article1.Comments.Add(new Comment()
        {
            Message = "message2 for article 2",
        });
        ctx.Article.Add(article1);
        ctx.SaveChanges();
    }
}