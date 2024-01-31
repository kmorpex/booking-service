namespace GoVilla.Domain.Bookings;

public record BookingId(Guid Value)
{
    public static BookingId FromValue(Guid Value)
    {
        return new BookingId(Value);
    }

    public static BookingId New()
    {
        return FromValue(Guid.NewGuid());
    }
}