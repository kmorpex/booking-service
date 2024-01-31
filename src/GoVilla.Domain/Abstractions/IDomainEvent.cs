using MediatR;

namespace GoVilla.Domain.Abstractions;

// MediaR notifications are used to implement the Publish-Subscribe pattern
public interface IDomainEvent : INotification
{
}