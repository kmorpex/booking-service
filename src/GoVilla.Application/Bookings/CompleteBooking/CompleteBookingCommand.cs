using GoVilla.Application.Abstractions.Messaging;

namespace GoVilla.Application.Bookings.CompleteBooking;

public sealed record CompleteBookingCommand(Guid BookingId) : ICommand;