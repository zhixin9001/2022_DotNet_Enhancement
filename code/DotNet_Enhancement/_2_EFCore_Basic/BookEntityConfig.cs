using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace _2_EFCore_Basic;

public class BookEntityConfig: IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.ToTable("T_Books");
        builder.Property(e => e.Title).HasMaxLength(50).IsRequired();
        builder.Property(e => e.AuthorName).HasMaxLength(20).IsRequired();
        builder.HasQueryFilter(a => a.Id > 3);
    }
}