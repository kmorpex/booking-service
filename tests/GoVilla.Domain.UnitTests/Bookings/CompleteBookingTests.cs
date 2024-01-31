using FluentAssertions;
using GoVilla.Domain.Apartments;
using GoVilla.Domain.Apartments.Enums;
using GoVilla.Domain.Apartments.ValueObjects;
using GoVilla.Domain.Bookings;
using GoVilla.Domain.Bookings.Events;
using GoVilla.Domain.Bookings.ValueObjects;
using GoVilla.Domain.Shared.ValueObjects;
using GoVilla.Domain.Users;

namespace GoVilla.Domain.UnitTests.Bookings;

public class CompleteBookingTests : BaseTest
{
    private Apartment CreateApartment()
    {
        return new Apartment(
            ApartmentId.New(),
            new Name("Sherlock Holmes Apartment"),
            new Description("The apartment is located in the heart of London, in the City district, in a quiet and peaceful area."),
            new Address("England", "London", "E1 7PZ", "London", "Baker Street 221B"),
            new Money(3500, Currency.Eur),
            new Money(50, Currency.Eur),
            new List<Amenity>());
    }
    
    private Booking BookApartment(DateTime startDate, DateTime endDate)
    {
        var apartment = CreateApartment();
        return Booking.Reserve(
            apartment,
            UserId.New(),
            DateRange.Create(DateOnly.FromDateTime(startDate), DateOnly.FromDateTime(endDate)),
            DateTime.UtcNow,
            new PricingService());
    }
    
    [Fact]
    public void Complete_Should_Raise_Booking_Completed_Domain_Event()
    {
        // Arrange
        var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));
        booking.Confirm(DateTime.UtcNow);

        // Act
        booking.Complete(DateTime.UtcNow);

        // Assert
        var bookingCompletedDomainEvent = AssertDomainEventWasPublished<BookingCompletedDomainEvent>(booking);

        bookingCompletedDomainEvent.BookingId.Should().Be(booking.Id);
    }
    
    [Fact]
    public void Complete_After_Rejection_Should_Return_Failure()
    {
        // Arrange
        var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));
        booking.Reject(DateTime.UtcNow);

        // Act
        var result = booking.Complete(DateTime.UtcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BookingErrors.NotConfirmed);
    }
}