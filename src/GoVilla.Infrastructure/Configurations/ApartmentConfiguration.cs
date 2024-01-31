using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GoVilla.Domain.Apartments;
using GoVilla.Domain.Apartments.ValueObjects;
using GoVilla.Domain.Shared.ValueObjects;

namespace GoVilla.Infrastructure.Configurations;

internal sealed class ApartmentConfiguration : IEntityTypeConfiguration<Apartment>
{
    public void Configure(EntityTypeBuilder<Apartment> builder)
    {
        builder.ToTable("apartments");

        builder.HasKey(apartment => apartment.Id);

        // Define a conversion from the strongly typed id into the Guid value and also from the database value into the strongly typed id
        builder.Property(apartment => apartment.Id)
            .HasConversion(apartment => apartment.Value, value => new ApartmentId(value));

        builder.OwnsOne(apartment =>
            apartment.Address); // The value object is going to be mapped into a set of columns in the same table as the current entity

        builder.Property(apartment => apartment.Name)
            .HasMaxLength(200)
            .HasConversion(name => name.Value, value => new Name(value));

        builder.Property(apartment => apartment.Description)
            .HasMaxLength(2000)
            .HasConversion(description => description.Value, value => new Description(value));

        builder.OwnsOne(apartment => apartment.Price,
            priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.GetByCode(code));
            });

        builder.OwnsOne(apartment => apartment.CleaningFee,
            priceBuilder =>
            {
                priceBuilder.Property(money => money.Currency)
                    .HasConversion(currency => currency.Code, code => Currency.GetByCode(code));
            });

        // Define a shadow property and instruct EF Core to interpret this column as a row version for implementing optimistic concurrency support
        builder.Property<uint>("Version").IsRowVersion();
    }
}