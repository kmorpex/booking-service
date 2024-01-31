using GoVilla.Application.Abstractions.Clock;
using GoVilla.Application.Abstractions.Messaging;
using GoVilla.Domain.Abstractions;
using GoVilla.Domain.Bookings;

namespace GoVilla.Application.Bookings.RejectBooking;

public sealed class RejectBookingCommandHandler : ICommandHandler<RejectBookingCommand>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IBookingRepository _bookingRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RejectBookingCommandHandler(IDateTimeProvider dateTimeProvider, IBookingRepository bookingRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingRepository = bookingRepository;
        _unitOfWork = unitOfWork;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
    {
        var booking = await _bookingRepository.GetByIdAsync(new BookingId(request.BookingId), cancellationToken);
        if (booking is null) return Result.Failure(BookingErrors.NotFound);

        var result = booking.Reject(_dateTimeProvider.UtcNow);
        if (result.IsFailure) return result;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}