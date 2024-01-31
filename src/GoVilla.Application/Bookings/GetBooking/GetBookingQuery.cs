using GoVilla.Application.Abstractions.Messaging;

namespace GoVilla.Application.Bookings.GetBooking;

public sealed record GetBookingQuery(Guid BookingId) : IQuery<BookingResponse>;