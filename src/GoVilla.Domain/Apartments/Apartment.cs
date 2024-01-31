using GoVilla.Domain.Abstractions;
using GoVilla.Domain.Apartments.Enums;
using GoVilla.Domain.Apartments.ValueObjects;
using GoVilla.Domain.Shared.ValueObjects;

namespace GoVilla.Domain.Apartments;

public sealed class Apartment : Entity<ApartmentId>
{
    // The private setter is used because we don't want to allow the property to be changed outside the scope of the entity
    public Name Name { get; private set; }
    public Description Description { get; private set; }
    public Address Address { get; private set; }
    public Money Price { get; private set; }
    public Money CleaningFee { get; private set; }

    // We can only set the value of this property in the domain project
    public DateTime? LastBookedOnUtc { get; internal set; }

    public List<Amenity> Amenities { get; private set; } = new();

    public Apartment(ApartmentId id, Name name, Description description, Address address, Money price,
        Money cleaningFee, List<Amenity> amenities) : base(id)
    {
        Name = name;
        Description = description;
        Address = address;
        Price = price;
        CleaningFee = cleaningFee;
        Amenities = amenities;
    }

    private Apartment()
    {
    }
}