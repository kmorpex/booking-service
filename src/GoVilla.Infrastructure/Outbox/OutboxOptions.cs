namespace GoVilla.Infrastructure.Outbox;

public sealed class OutboxOptions
{
    public int IntervalInSeconds { get; init; } // Determines how often our background job will execute

    public int
        BatchSize
    {
        get;
        init;
    } // Number of outbox messages to be processed in a single execution of the background job
}