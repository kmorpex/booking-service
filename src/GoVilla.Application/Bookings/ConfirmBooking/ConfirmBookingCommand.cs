using GoVilla.Application.Abstractions.Messaging;

namespace GoVilla.Application.Bookings.ConfirmBooking;

public sealed record ConfirmBookingCommand(Guid BookingId) : ICommand;