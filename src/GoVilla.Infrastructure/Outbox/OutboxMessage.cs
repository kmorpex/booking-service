namespace GoVilla.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; private set; }
    public DateTime OccurredOnUtc { get; private set; }
    public string Type { get; private set; } // Name of our domain event
    public string Content { get; private set; } // Serialized domain event
    public DateTime? ProcessedOnUtc { get; private set; }
    public string? Error { get; private set; }

    public OutboxMessage(Guid id, DateTime occurredOnUtc, string type, string content)
    {
        Id = id;
        OccurredOnUtc = occurredOnUtc;
        Type = type;
        Content = content;
    }
}