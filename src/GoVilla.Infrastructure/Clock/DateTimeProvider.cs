using GoVilla.Application.Abstractions.Clock;

namespace GoVilla.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}