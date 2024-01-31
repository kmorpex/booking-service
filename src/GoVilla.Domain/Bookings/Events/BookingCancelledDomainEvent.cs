using GoVilla.Domain.Abstractions;

namespace GoVilla.Domain.Bookings.Events;

public sealed record BookingCancelledDomainEvent(BookingId BookingId) : IDomainEvent;