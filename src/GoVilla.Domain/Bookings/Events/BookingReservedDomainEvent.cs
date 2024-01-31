using GoVilla.Domain.Abstractions;

namespace GoVilla.Domain.Bookings.Events;

public sealed record BookingReservedDomainEvent(BookingId BookingId) : IDomainEvent;