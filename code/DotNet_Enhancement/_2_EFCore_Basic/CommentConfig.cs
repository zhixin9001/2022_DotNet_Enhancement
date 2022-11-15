using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2_EFCore_Basic;

public class CommentConfig: IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .HasOne(a => a.Article)
            .WithMany(c => c.Comments)
            .IsRequired()
            .HasForeignKey(a=>a.ArticleId);
    }
}