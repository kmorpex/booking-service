using GoVilla.Application.Abstractions.Email;
using GoVilla.Domain.Bookings;
using GoVilla.Domain.Bookings.Events;
using GoVilla.Domain.Users;
using MediatR;

namespace GoVilla.Application.Bookings.ReserveBooking;

public sealed class BookingReservedDomainEventHandler : INotificationHandler<BookingReservedDomainEvent>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public BookingReservedDomainEventHandler(IBookingRepository bookingRepository, IUserRepository userRepository,
        IEmailService emailService)
    {
        _bookingRepository = bookingRepository;
        _userRepository = userRepository;
        _emailService = emailService;
    }

    // We want to send an email notification to the user who just reserved this apartment
    public async Task Handle(BookingReservedDomainEvent notification, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(notification.BookingId, cancellationToken);
        if (booking is null) return;

        var user = await _userRepository.GetByIdAsync(booking.UserId, cancellationToken);
        if (user is null) return;

        await _emailService.SendAsync(user.Email, "Details of your new Booking",
            "You have 10 minutes to confirm this booking");
    }
}