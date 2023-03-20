using FileService.Domain;
using Microsoft.Extensions.Options;

namespace FileService.Infrastructure;

public class MockCloudStorageClient : SMBStorageClient
{
    public override StorageType StorageType => StorageType.Public;

    public MockCloudStorageClient(IOptionsSnapshot<MockCloudStorageOptions> options):base(options)
    {
    }
}