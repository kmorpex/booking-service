using GoVilla.Domain.Apartments;
using GoVilla.Domain.Bookings;
using GoVilla.Domain.Bookings.Enums;
using GoVilla.Domain.Bookings.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GoVilla.Infrastructure.Repositories;

internal sealed class BookingRepository : Repository<Booking, BookingId>, IBookingRepository
{
    private static readonly BookingStatus[] ActiveBookingStatuses =
    {
        BookingStatus.Reserved,
        BookingStatus.Confirmed,
        BookingStatus.Completed
    };

    public BookingRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> IsOverlappingAsync(Apartment apartment, DateRange duration,
        CancellationToken cancellationToken = default)
    {
        return await DbContext
            .Set<Booking>()
            .AnyAsync(
                booking =>
                    booking.ApartmentId == apartment.Id &&
                    booking.Duration.Start <= duration.End &&
                    booking.Duration.End >= duration.Start &&
                    ActiveBookingStatuses.Contains(booking.Status),
                cancellationToken);
    }
}