using DomainCommons.Models;

namespace FileService.Domain.Entities;

public record UploadedItem : BaseEntity
{
    public DateTime CreationTime { get; private set; }
    public long FileSizeInBytes { get; private set; }
    public string FileName { get; private set; }
    public string FileSHA256Hash { get; private set; }
    public Uri BackupUrl { get; private set; }
    public Uri RemoteUrl { get; private set; }

    public static UploadedItem Create(Guid id, long fileSizeInBytes, string fileName, string fileSHA256Hash,
        Uri backupUrl, Uri remoteUrl)
    {
        return new UploadedItem
        {
            FileSizeInBytes = fileSizeInBytes,
            FileName = fileName,
            FileSHA256Hash = fileSHA256Hash,
            BackupUrl = backupUrl,
            RemoteUrl = remoteUrl
        };
    }
}