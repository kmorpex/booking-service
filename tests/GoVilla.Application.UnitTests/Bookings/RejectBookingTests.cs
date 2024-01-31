using FluentAssertions;
using GoVilla.Application.Abstractions.Clock;
using GoVilla.Application.Bookings.RejectBooking;
using GoVilla.Domain.Abstractions;
using GoVilla.Domain.Apartments;
using GoVilla.Domain.Apartments.Enums;
using GoVilla.Domain.Apartments.ValueObjects;
using GoVilla.Domain.Bookings;
using GoVilla.Domain.Bookings.ValueObjects;
using GoVilla.Domain.Shared.ValueObjects;
using GoVilla.Domain.Users;
using GoVilla.Domain.Users.ValueObjects;
using Moq;

namespace GoVilla.Application.UnitTests.Bookings;

public class RejectBookingTests 
{
    private static Booking BookApartment()
    {
        var apartment = new Apartment(
            ApartmentId.New(),
            new Name("Sherlock Holmes Apartment"),
            new Description("The apartment is located in the heart of London, in the City district, in a quiet and peaceful area."),
            new Address("England", "London", "E1 7PZ", "London", "Baker Street 221B"),
            new Money(3500, Currency.Eur),
            new Money(50, Currency.Eur),
            new List<Amenity>());
        
        return Booking.Reserve(
            apartment,
            UserId.New(),
            DateRange.Create(DateOnly.FromDateTime(DateTime.UtcNow), DateOnly.FromDateTime(DateTime.UtcNow.AddDays(15))),
            DateTime.UtcNow,
            new PricingService());
    }
    
    [Fact]
    public async Task Handle_Should_Return_Success_When_Booking_Is_Reserved()
    {
        // Arrange
        var booking = BookApartment();
        var command = new RejectBookingCommand(booking.Id.Value);

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        bookingRepositoryMock
            .Setup(u => u.GetByIdAsync(It.IsAny<BookingId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(booking);

        var handler = new RejectBookingCommandHandler(
            new Mock<IDateTimeProvider>().Object,
            bookingRepositoryMock.Object,
            new Mock<IUnitOfWork>().Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task Handle_Should_Return_Failure_When_Booking_Is_Null()
    {
        // Arrange
        var command = new RejectBookingCommand(Guid.NewGuid());

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        bookingRepositoryMock
            .Setup(u => u.GetByIdAsync(It.IsAny<BookingId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Booking?)null);

        var handler = new RejectBookingCommandHandler(
            new Mock<IDateTimeProvider>().Object,
            bookingRepositoryMock.Object,
            new Mock<IUnitOfWork>().Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Error.Should().Be(BookingErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_Return_Error_When_Booking_IsNot_Reserved()
    {
        // Arrange
        var booking = BookApartment();
        booking.Confirm(DateTime.UtcNow);
        var command = new RejectBookingCommand(booking.Id.Value);

        var bookingRepositoryMock = new Mock<IBookingRepository>();
        bookingRepositoryMock
            .Setup(u => u.GetByIdAsync(It.IsAny<BookingId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(booking);

        var handler = new RejectBookingCommandHandler(
            new Mock<IDateTimeProvider>().Object,
            bookingRepositoryMock.Object,
            new Mock<IUnitOfWork>().Object);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.Error.Should().Be(BookingErrors.NotReserved);
    }
}