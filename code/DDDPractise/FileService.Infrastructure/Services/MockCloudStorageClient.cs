using FileService.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace FileService.Infrastructure.Services;

public class MockCloudStorageClient : IStorageClient
{
    private readonly IWebHostEnvironment _hostEnv;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public StorageType StorageType => StorageType.Public;

    public MockCloudStorageClient(IWebHostEnvironment hostEnv, IHttpContextAccessor httpContextAccessor)
    {
        _hostEnv = hostEnv;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
    {
        if (key.StartsWith('/'))
        {
            throw new ArgumentException("key should not start with /");
        }

        var workingDir = Path.Combine(_hostEnv.ContentRootPath, "wwwroot");
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
        var req = _httpContextAccessor.HttpContext.Request;
        var url = req.Scheme + "://" + req.Host + "/FileService" + key;
        return new Uri(url);
    }
}