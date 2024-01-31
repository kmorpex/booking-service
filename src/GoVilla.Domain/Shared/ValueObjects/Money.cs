namespace GoVilla.Domain.Shared.ValueObjects;

public record Money(decimal Amount, Currency Currency)
{
    public static Money operator +(Money a, Money b)
    {
        if (a.Currency != b.Currency) throw new InvalidOperationException("Currencies have to be equal");

        return new Money(a.Amount + b.Amount, a.Currency);
    }

    public static Money Zero()
    {
        return new Money(0, Currency.None);
    }

    public static Money Zero(Currency currency)
    {
        return new Money(0, currency);
    }

    public bool IsZero()
    {
        return this == Zero(Currency);
    }
}