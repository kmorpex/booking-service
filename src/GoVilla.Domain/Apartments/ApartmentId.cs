namespace GoVilla.Domain.Apartments;

public record ApartmentId(Guid Value)
{
    public static ApartmentId FromValue(Guid Value)
    {
        return new ApartmentId(Value);
    }

    public static ApartmentId New()
    {
        return FromValue(Guid.NewGuid());
    }
}