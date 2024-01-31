using GoVilla.Domain.Abstractions;

namespace GoVilla.Domain.Reviews.Events;

public sealed record ReviewCreatedDomainEvent(ReviewId ReviewId) : IDomainEvent;