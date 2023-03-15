using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure.Configs;

public class UploadedItemConfig : IEntityTypeConfiguration<UploadedItem>
{
    public void Configure(EntityTypeBuilder<UploadedItem> builder)
    {
        builder.ToTable("T_FS_UploadedItems");
        builder.Property(a => a.FileName).IsUnicode().HasMaxLength(1024);
        builder.Property(a => a.FileSHA256Hash).IsUnicode(false).HasMaxLength(64);
        builder.HasIndex(e => new {e.FileSHA256Hash, e.FileSizeInBytes});
    }
}