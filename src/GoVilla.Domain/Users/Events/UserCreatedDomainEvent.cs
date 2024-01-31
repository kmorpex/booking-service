using GoVilla.Domain.Abstractions;

namespace GoVilla.Domain.Users.Events;

public sealed record class UserCreatedDomainEvent(UserId UserId) : IDomainEvent;