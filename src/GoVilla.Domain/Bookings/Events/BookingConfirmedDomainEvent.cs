using GoVilla.Domain.Abstractions;

namespace GoVilla.Domain.Bookings.Events;

public sealed record BookingConfirmedDomainEvent(BookingId BookingId) : IDomainEvent;