namespace GoVilla.Domain.Users;

public record UserId(Guid Value)
{
    public static UserId FromValue(Guid Value)
    {
        return new UserId(Value);
    }

    public static UserId New()
    {
        return FromValue(Guid.NewGuid());
    }
}