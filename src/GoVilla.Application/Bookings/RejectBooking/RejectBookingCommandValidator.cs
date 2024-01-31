using FluentValidation;

namespace GoVilla.Application.Bookings.RejectBooking;

public class RejectBookingCommandValidator : AbstractValidator<RejectBookingCommand>
{
    public RejectBookingCommandValidator()
    {
        RuleFor(c => c.BookingId).NotEmpty();
    }
}