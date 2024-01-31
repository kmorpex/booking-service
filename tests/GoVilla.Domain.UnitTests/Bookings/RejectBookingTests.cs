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

public class RejectBookingTests : BaseTest
{
    private Apartment CreateApartment()
    {
        return new Apartment(
            ApartmentId.New(),
            new Name("New York Apartment"),
            new Description("The apartment is located in the heart of New York, in the City district, in a quiet and peaceful area."),
            new Address("USA", "NY", "10012", "New York", "Wall Street 100"),
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
    public void Reject_Should_Raise_Booking_Rejected_Domain_Event()
    {
        // Arrange
        var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));

        // Act
        booking.Reject(DateTime.UtcNow);

        // Assert
        var bookingRejectedDomainEvent = AssertDomainEventWasPublished<BookingRejectedDomainEvent>(booking);

        bookingRejectedDomainEvent.BookingId.Should().Be(booking.Id);
    }

    [Fact]
    public void Reject_After_Rejection_Should_Return_Failure()
    {
        // Arrange
        var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));
        booking.Reject(DateTime.UtcNow);

        // Act
        var result = booking.Reject(DateTime.UtcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BookingErrors.NotReserved);
    }
    
    [Fact]
    public void Reject_Before_Confirmation_Should_Return_Failure()
    {
        // Arrange
        var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));
        booking.Confirm(DateTime.UtcNow);

        // Act
        var result = booking.Reject(DateTime.UtcNow);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(BookingErrors.NotReserved);
    }
}