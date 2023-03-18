using Microsoft.Extensions.DependencyInjection;

namespace Zhixin.Commons;

public interface IModuleInitializer
{
    public void Initialize(IServiceCollection services);
}