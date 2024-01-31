using GoVilla.Domain.Abstractions;

namespace GoVilla.Domain.Bookings.Events;

public sealed record BookingRejectedDomainEvent(BookingId BookingId) : IDomainEvent;