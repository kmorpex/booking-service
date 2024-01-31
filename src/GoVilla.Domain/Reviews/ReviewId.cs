namespace GoVilla.Domain.Reviews;

public record ReviewId(Guid Value)
{
    public static ReviewId FromValue(Guid Value)
    {
        return new ReviewId(Value);
    }

    public static ReviewId New()
    {
        return FromValue(Guid.NewGuid());
    }
}