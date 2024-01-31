using GoVilla.Application.Abstractions.Clock;
using GoVilla.Application.Abstractions.Messaging;
using GoVilla.Domain.Abstractions;
using GoVilla.Domain.Bookings;

namespace GoVilla.Application.Bookings.ConfirmBooking;

public sealed class ConfirmBookingCommandHandler : ICommandHandler<ConfirmBookingCommand>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ConfirmBookingCommandHandler(IDateTimeProvider dateTimeProvider, IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork)
    {
        _dateTimeProvider = dateTimeProvider;
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(new BookingId(request.BookingId), cancellationToken);
        if (booking is null) return Result.Failure(BookingErrors.NotFound);

        var result = booking.Confirm(_dateTimeProvider.UtcNow);
        if (result.IsFailure) return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}