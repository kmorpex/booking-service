using System.Data;
using System.Net.Mime;
using Dapper;
using FluentAssertions;
using Moq;
using GoVilla.Application.Abstractions.Data;
using GoVilla.Application.Bookings.GetBooking;
using GoVilla.Domain.Abstractions;
using GoVilla.Domain.Apartments;
using GoVilla.Domain.Apartments.Enums;
using GoVilla.Domain.Apartments.ValueObjects;
using GoVilla.Domain.Bookings;
using GoVilla.Domain.Bookings.ValueObjects;
using GoVilla.Domain.Shared.ValueObjects;
using GoVilla.Domain.Users;
using Moq.Dapper;

namespace GoVilla.Application.UnitTests.Bookings;

public class GetBookingTests
{
    private readonly Booking _booking = Booking.Reserve(
        new Apartment(
            ApartmentId.New(),
            new Name("Sherlock Holmes Apartment"),
            new Description("The apartment is located in the heart of London, in the City district, in a quiet and peaceful area."),
            new Address("England", "London", "E1 7PZ", "London", "Baker Street 221B"),
            new Money(3500, Currency.Eur),
            new Money(50, Currency.Eur),
            new List<Amenity>()),
        UserId.New(),
        DateRange.Create(DateOnly.FromDateTime(new DateTime(2024, 10, 12)), DateOnly.FromDateTime(new DateTime(2024, 10, 18))),
        DateTime.UtcNow,
        new PricingService());
    
    [Fact]
    public async Task Handle_Should_Return_BookingResponse()
    {
        // Arrange
        var mockConnectionFactory = new Mock<ISqlConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();
        
        // Set up the mock connection to return a BookingResponse
        mockConnection
            .SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<BookingResponse>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
            .ReturnsAsync(new BookingResponse()
            {
                Id = _booking.Id.Value,
                ApartmentId = _booking.ApartmentId.Value,
                UserId = _booking.UserId.Value,
                Status = (int)_booking.Status,
                DurationStart = _booking.Duration.Start,
                DurationEnd = _booking.Duration.End,
            });

        mockConnectionFactory.Setup(x => x.CreateConnection()).Returns(mockConnection.Object);

        var handler = new GetBookingQueryHandler(mockConnectionFactory.Object);
        var query = new GetBookingQuery(_booking.Id.Value);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        result.Value.Should().BeOfType<BookingResponse>();
    }
    
    [Fact]
    public async Task Handle_Should_Return_NotFound()
    {
        // Arrange
        var mockConnectionFactory = new Mock<ISqlConnectionFactory>();
        var mockConnection = new Mock<IDbConnection>();
        
        // Set up the mock connection to return a BookingResponse
        mockConnection
            .SetupDapperAsync(c => c.QueryFirstOrDefaultAsync<BookingResponse?>(
                It.IsAny<string>(),
                It.IsAny<object>(),
                It.IsAny<IDbTransaction>(),
                It.IsAny<int?>(),
                It.IsAny<CommandType?>()))
            .ReturnsAsync(null as BookingResponse);

        mockConnectionFactory.Setup(x => x.CreateConnection()).Returns(mockConnection.Object);

        var handler = new GetBookingQueryHandler(mockConnectionFactory.Object);
        var query = new GetBookingQuery(_booking.Id.Value);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
    }
}