using GoVilla.Domain.Apartments;
using GoVilla.Domain.Bookings;
using GoVilla.Domain.Shared.ValueObjects;
using GoVilla.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GoVilla.Infrastructure.Configurations;

internal sealed class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("bookings");

        builder.HasKey(booking => booking.Id);

        builder.Property(booking => booking.Id)
            .HasConversion(booking => booking.Value, value => new BookingId(value));

        builder.OwnsOne(booking => booking.PriceForPeriod,
            priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.GetByCode(code));
            });

        builder.OwnsOne(booking => booking.CleaningFee,
            priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.GetByCode(code));
            });

        builder.OwnsOne(booking => booking.AmenitiesUpCharge,
            priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.GetByCode(code));
            });

        builder.OwnsOne(booking => booking.TotalPrice,
            priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.GetByCode(code));
            });

        builder.OwnsOne(booking => booking.Duration);

        builder.HasOne<Apartment>() // A booking has one apartment associated with it
            .WithMany() // An apartment can have many bookings
            .HasForeignKey(booking => booking.ApartmentId);

        builder.HasOne<User>() // A booking has one user associated with it
            .WithMany() // An user can have many bookings
            .HasForeignKey(booking => booking.UserId);
    }
}