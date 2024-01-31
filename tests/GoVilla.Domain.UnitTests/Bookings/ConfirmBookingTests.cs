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

public class ConfirmBookingTests : BaseTest
{
    private Apartment CreateApartment()
    {
        return new Apartment(
            ApartmentId.New(),
            new Name("Old Town Apartment"),
            new Description("Description here"),
            new Address("USA", "MS", "02110", "Boston", "Washington Street"),
            new Money(1000, Currency.Usd),
            new Money(50, Currency.Usd),
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
    public void Confirm_Should_Raise_Booking_Confirmed_Domain_Event()
    {
        // Arrange
        var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));

        // Act
        booking.Confirm(DateTime.UtcNow);

        // Assert
        var bookingConfirmedDomainEvent = AssertDomainEventWasPublished<BookingConfirmedDomainEvent>(booking);

        bookingConfirmedDomainEvent.BookingId.Should().Be(booking.Id);
    }
    
    [Fact]
    public void Confirm_Before_Reservation_Should_Return_Failure()
    {
        // Arrange
        var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));

        // Act
        booking.Confirm(DateTime.UtcNow);

        // Assert
        var bookingConfirmedDomainEvent = AssertDomainEventWasPublished<BookingConfirmedDomainEvent>(booking);

        bookingConfirmedDomainEvent.BookingId.Should().Be(booking.Id);
    }
    
    [Fact]
    public void Confirm_After_Reservation_Should_Return_Failure()
    {
        // Arrange
        var booking = BookApartment(new DateTime(2024, 10, 12), new DateTime(2024, 10, 18));

        // Act
        booking.Confirm(DateTime.UtcNow);

        // Assert
        var bookingConfirmedDomainEvent = AssertDomainEventWasPublished<BookingConfirmedDomainEvent>(booking);

        bookingConfirmedDomainEvent.BookingId.Should().Be(booking.Id);
    }
}