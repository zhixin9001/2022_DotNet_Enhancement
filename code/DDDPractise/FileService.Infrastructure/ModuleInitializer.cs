using FileService.Domain;
using FileService.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Zhixin.Commons;

namespace FileService.Infrastructure;

public class ModuleInitializer : IModuleInitializer
{
    public void Initialize(IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<IStorageClient, SMBStorageClient>();
        services.AddScoped<IStorageClient, MockCloudStorageClient>();
        services.AddScoped<IFSRepository, FSRepository>();
        services.AddScoped<FSDomainService>();
        services.AddHttpClient();
    }
}