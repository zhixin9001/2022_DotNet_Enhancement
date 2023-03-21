using IdentityService.Domain;
using Microsoft.Extensions.Logging;

namespace IdDomainService.Infrastructure;

public class MockSmsSender : ISmsSender
{
    private readonly ILogger<MockSmsSender> _logger;

    public MockSmsSender(ILogger<MockSmsSender> logger)
    {
        _logger = logger;
    }

    public Task SendAsync(string phoneNum, params string[] args)
    {
        _logger.LogInformation("Send sms to {0}, args: {1}", phoneNum, string.Join(",", args));
        return Task.CompletedTask;
    }
}