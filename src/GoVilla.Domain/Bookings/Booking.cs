using GoVilla.Domain.Abstractions;
using GoVilla.Domain.Apartments;
using GoVilla.Domain.Bookings.Enums;
using GoVilla.Domain.Bookings.Events;
using GoVilla.Domain.Bookings.ValueObjects;
using GoVilla.Domain.Shared.ValueObjects;
using GoVilla.Domain.Users;

namespace GoVilla.Domain.Bookings;

public sealed class Booking : Entity<BookingId>
{
    public ApartmentId ApartmentId { get; private set; }
    public UserId UserId { get; private set; }
    public DateRange Duration { get; private set; }
    public Money PriceForPeriod { get; private set; }
    public Money CleaningFee { get; private set; }
    public Money AmenitiesUpCharge { get; private set; }
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }
    public DateTime? ConfirmedOnUtc { get; private set; }
    public DateTime? RejectedOnUtc { get; private set; }
    public DateTime? CompletedOnUtc { get; private set; }
    public DateTime? CancelledOnUtc { get; private set; }

    private Booking(
        BookingId id,
        ApartmentId apartmentId,
        UserId userId,
        DateRange duration,
        Money priceForPeriod,
        Money cleaningFee,
        Money amenitiesUpCharge,
        Money totalPrice,
        BookingStatus status,
        DateTime createdOnUtc)
        : base(id)
    {
        ApartmentId = apartmentId;
        UserId = userId;
        Duration = duration;
        PriceForPeriod = priceForPeriod;
        CleaningFee = cleaningFee;
        AmenitiesUpCharge = amenitiesUpCharge;
        TotalPrice = totalPrice;
        Status = status;
        CreatedOnUtc = createdOnUtc;
    }

    private Booking()
    {
    }

    public static Booking Reserve(Apartment apartment, UserId userId, DateRange duration, DateTime utcNow,
        PricingService pricingService)
    {
        var pricingDetails = pricingService.CalculatePrice(apartment, duration);
        var booking = new Booking(
            BookingId.New(),
            apartment.Id,
            userId,
            duration,
            pricingDetails.PriceForPeriod,
            pricingDetails.CleaningFee,
            pricingDetails.AmenitiesUpCharge,
            pricingDetails.TotalPrice,
            BookingStatus.Reserved,
            utcNow);
        booking.RaiseDomainEvent(new BookingReservedDomainEvent(booking.Id));
        apartment.LastBookedOnUtc = utcNow;

        return booking;
    }

    public Result Confirm(DateTime utcNow)
    {
        var propertyHasBeenReserved = Status == BookingStatus.Reserved;
        if (!propertyHasBeenReserved)
            return Result.Failure(BookingErrors.NotReserved);

        Status = BookingStatus.Confirmed;
        ConfirmedOnUtc = utcNow;
        RaiseDomainEvent(new BookingConfirmedDomainEvent(Id));

        return Result.Success();
    }
    
    public Result Reject(DateTime utcNow)
    {
        var propertyHasBeenReserved = Status == BookingStatus.Reserved;
        if (!propertyHasBeenReserved)
            return Result.Failure(BookingErrors.NotReserved);

        Status = BookingStatus.Rejected;
        RejectedOnUtc = utcNow;
        RaiseDomainEvent(new BookingRejectedDomainEvent(Id));

        return Result.Success();
    }

    public Result Complete(DateTime utcNow)
    {
        var bookingHasBeenConfirmed = Status == BookingStatus.Confirmed;
        if (!bookingHasBeenConfirmed)
            return Result.Failure(BookingErrors.NotConfirmed);

        Status = BookingStatus.Completed;
        CompletedOnUtc = utcNow;
        RaiseDomainEvent(new BookingCompletedDomainEvent(Id));

        return Result.Success();
    }

    public Result Cancel(DateTime utcNow)
    {
        var bookingHasBeenConfirmed = Status == BookingStatus.Confirmed;
        if (!bookingHasBeenConfirmed)
            return Result.Failure(BookingErrors.NotConfirmed);

        var currentDate = DateOnly.FromDateTime(utcNow);
        var bookingPeriodAlreadyStarted = currentDate > Duration.Start;
        if (bookingPeriodAlreadyStarted)
            return Result.Failure(BookingErrors.AlreadyStarted);

        Status = BookingStatus.Cancelled;
        CancelledOnUtc = utcNow;
        RaiseDomainEvent(new BookingCancelledDomainEvent(Id));

        return Result.Success();
    }
}