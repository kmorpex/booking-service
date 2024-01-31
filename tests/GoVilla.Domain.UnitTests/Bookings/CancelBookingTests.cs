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

public class CancelBookingTests : BaseTest
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
    public void Cancel_Should_Raise_Booking_Cancelled_Domain_Event()
    {
        // Arrange
        var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));
        booking.Confirm(DateTime.UtcNow);

        // Act
        booking.Cancel(DateTime.UtcNow);

        // Assert
        var bookingCancelledDomainEvent = AssertDomainEventWasPublished<BookingCancelledDomainEvent>(booking);

        bookingCancelledDomainEvent.BookingId.Should().Be(booking.Id);
    }

    [Fact]
    public void Cancel_Before_Confirmation_Should_Return_Failure()
    {
        // Arrange
        var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));

        // Act
        var result = booking.Cancel(DateTime.UtcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void Cancel_After_Start_Date_Should_Return_Failure()
    {
        // Arrange
        var booking = BookApartment(new DateTime(2023, 05, 10), new DateTime(2024, 06, 10));

        // Act
        var result = booking.Cancel(DateTime.UtcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
    }
}