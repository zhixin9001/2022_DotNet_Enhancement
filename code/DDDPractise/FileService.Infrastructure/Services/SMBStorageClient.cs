using FileService.Domain;
using Microsoft.Extensions.Options;

namespace FileService.Infrastructure.Services;

public class SMBStorageClient : IStorageClient
{
    private readonly IOptionsSnapshot<SMBStorageOptions> _options;
    public StorageType StorageType => StorageType.Backup;

    public SMBStorageClient(IOptionsSnapshot<SMBStorageOptions> options)
    {
        _options = options;
    }

    public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
    {
        if (key.StartsWith('/'))
        {
            throw new ArgumentException("key should not start with /");
        }

        var workingDir = _options.Value.WorkingDir;
        var fullPath = Path.Combine(workingDir, key);
        var fullDir = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(fullDir))
        {
            Directory.CreateDirectory(fullDir);
        }

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        using Stream outStream = File.OpenWrite(fullPath);
        await content.CopyToAsync(outStream, cancellationToken);
        return new Uri(fullPath);
    }
}