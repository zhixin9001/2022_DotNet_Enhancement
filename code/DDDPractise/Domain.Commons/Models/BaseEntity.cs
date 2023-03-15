namespace DomainCommons.Models;

public record BaseEntity : IEntity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
}