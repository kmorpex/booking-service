using FluentValidation;

namespace GoVilla.Application.Bookings.ConfirmBooking;

public class ConfirmBookingCommandValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingCommandValidator()
    {
        RuleFor(c => c.BookingId).NotEmpty();
    }
}