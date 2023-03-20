using FileService.Domain;
using FileService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure;

public class FSRepository : IFSRepository
{
    private readonly FSDbContext _fsDbContext;

    public FSRepository(FSDbContext fsDbContext)
    {
        _fsDbContext = fsDbContext;
    }

    public Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash)
    {
        return _fsDbContext.UploadedItems.FirstOrDefaultAsync(a =>
            a.FileSizeInBytes == fileSize && a.FileSHA256Hash == sha256Hash);
    }

    public Task<UploadedItem?> FindFileAsync(string fileName)
    {
        return _fsDbContext.UploadedItems.FirstOrDefaultAsync(a => a.FileName == fileName);
    }

    public async Task<bool> AddFile(UploadedItem item)
    {
        _fsDbContext.UploadedItems.Add(item);
        return (await _fsDbContext.SaveChangesAsync()) > 0;
    }
}