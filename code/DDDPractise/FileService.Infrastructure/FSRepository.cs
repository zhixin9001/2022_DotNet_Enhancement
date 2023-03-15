using FileService.Domain;
using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure;

public class FSRepository : IFSRepository
{
    private readonly FSDbContext dbContext;

    public FSRepository(FSDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash)
    {
        return dbContext.UploadedItems.FirstOrDefaultAsync(u =>
            u.FileSizeInBytes == fileSize && u.FileSHA256Hash == sha256Hash);
    }
}