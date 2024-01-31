using GoVilla.Domain.Abstractions;

namespace GoVilla.Domain.Bookings.Events;

public sealed record BookingCompletedDomainEvent(BookingId BookingId) : IDomainEvent;