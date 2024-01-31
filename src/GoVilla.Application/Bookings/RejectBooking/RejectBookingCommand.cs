using GoVilla.Application.Abstractions.Messaging;

namespace GoVilla.Application.Bookings.RejectBooking;

public sealed record RejectBookingCommand(Guid BookingId) : ICommand;