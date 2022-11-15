namespace _2_EFCore_Basic;

public class Article
{
    public long Id { get; set; }
    public string Title { get; set; }
    public List<Comment> Comments { get; set; } = new();
}

public class Comment
{
    public long Id { get; set; }
    
    public long ArticleId { get; set; }
    public Article Article { get; set; }
    public string Message { get; set; }
}