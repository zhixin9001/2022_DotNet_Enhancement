using FileService.Domain.Entities;

namespace FileService.Domain;

public interface IFSRepository
{
    Task<UploadedItem?> FindFileAsync(long fileSize, string sha256Hash);
    Task<UploadedItem?> FindFileAsync(string fileName);
    Task<bool> AddFile(UploadedItem item);
}