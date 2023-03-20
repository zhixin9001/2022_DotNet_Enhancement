using FileService.Domain.Entities;
using Zhixin.Commons;

namespace FileService.Domain;

public class FSDomainService
{
    private readonly IFSRepository repository;
    private readonly IStorageClient backupStorage; //备份服务器
    private readonly IStorageClient remoteStorage; //文件存储服务器

    public FSDomainService(IFSRepository repository
        , IEnumerable<IStorageClient> storageClients)
    {
        this.repository = repository;
        this.backupStorage = storageClients.First(a => a.StorageType == StorageType.Backup);
        this.remoteStorage = storageClients.First(a => a.StorageType == StorageType.Public);
    }

    public async Task<UploadedItem> UploadAsync(Stream stream, string fileName, CancellationToken cancellationToken)
    {
        string hash = HashHelper.ComputeSha256Hash(stream);
        long fileSize = stream.Length;
        var today = DateTime.Today;
        var key = $"{today.Year}/{today.Month}/{today.Day}/{hash}/{fileName}";

        var oldUploadItem = await repository.FindFileAsync(fileSize, hash);
        if (oldUploadItem != null)
        {
            return oldUploadItem;
        }

        stream.Position = 0;
        var backupUrl = await backupStorage.SaveAsync(key, stream, cancellationToken);

        stream.Position = 0;
        var remoteUrl = await remoteStorage.SaveAsync(key, stream, cancellationToken);

        var uploadedItem = UploadedItem.Create(Guid.NewGuid(), fileSize, fileName, hash, backupUrl,
            backupUrl);
        await repository.AddFile(uploadedItem);
        return uploadedItem;
    }
}