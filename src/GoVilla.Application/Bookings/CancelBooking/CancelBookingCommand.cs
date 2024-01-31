using GoVilla.Application.Abstractions.Messaging;

namespace GoVilla.Application.Bookings.CancelBooking;

public sealed record CancelBookingCommand(Guid BookingId) : ICommand;