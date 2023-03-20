using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FileService.Infrastructure;

public class UploadedItemConfig : IEntityTypeConfiguration<UploadedItem>
{
    public void Configure(EntityTypeBuilder<UploadedItem> builder)
    {
        builder.ToTable("T_FS_UploadedItems");
        builder.Property(e => e.FileName).IsUnicode().HasMaxLength(1024);
        builder.Property(e => e.FileSHA256Hash).IsUnicode().HasMaxLength(64);
    }
}