using MediatR;

namespace IdentitiyService.WebAPI.Events;

public record UserCreatedEvent(Guid Id, string UserName, string Password, string PhoneNumber) : IRequest;

public class UserCreatedEventHandler : IRequestHandler<UserCreatedEvent>
{
    private readonly ISmsSender _smsSender;

    public UserCreatedEventHandler(ISmsSender smsSender)
    {
        _smsSender = smsSender;
    }

    public Task Handle(UserCreatedEvent request, CancellationToken cancellationToken)
    {
        _smsSender.SendAsync(request.PhoneNumber, request.Password, request.UserName);
        return Task.CompletedTask;
    }
}