using Quartz;
using Dapper;
using MediatR;
using System.Data;
using Newtonsoft.Json;
using GoVilla.Application.Abstractions.Clock;
using GoVilla.Application.Abstractions.Data;
using GoVilla.Domain.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GoVilla.Infrastructure.Outbox;

[DisallowConcurrentExecution] // We only want one instance of the background job running
internal sealed class ProcessOutboxMessagesJob : IJob
{
    private static readonly JsonSerializerSettings JsonSerializerSettings =
        new() { TypeNameHandling = TypeNameHandling.All };

    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly IPublisher _publisher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly OutboxOptions _outboxOptions;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        ISqlConnectionFactory sqlConnectionFactory,
        IPublisher publisher,
        IDateTimeProvider dateTimeProvider,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
        _publisher = publisher;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
        _outboxOptions = outboxOptions.Value;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Beginning to process outbox messages");

        using var connection = _sqlConnectionFactory.CreateConnection();
        using var transaction = connection.BeginTransaction();

        var outboxMessages = await GetOutboxMessagesAsync(connection, transaction);

        foreach (var outboxMessage in outboxMessages)
        {
            Exception? exception = null;
            try
            {
                var domainEvent =
                    JsonConvert.DeserializeObject<IDomainEvent>(outboxMessage.Content, JsonSerializerSettings)!;
                await _publisher.Publish(domainEvent,
                    context.CancellationToken); // Publish domain event and trigger respective event handler
            }
            catch (Exception caughtException)
            {
                _logger.LogError(caughtException, "Exception while processing outbox message {MessageId}",
                    outboxMessage.Id);
                exception = caughtException;
            }

            await UpdateOutboxMessageAsync(connection, transaction, outboxMessage, exception);
        }

        transaction.Commit();

        _logger.LogInformation("Completed processing outbox messages");
    }

    private async Task<IReadOnlyList<OutboxMessageResponse>> GetOutboxMessagesAsync(IDbConnection connection,
        IDbTransaction transaction)
    {
        // 'FOR UPDATE' locks the selected rows, preventing other transactions from modifying them.
        // This ensures that multiple background job instances don't process the same row simultaneously. The lock persists until the transaction is committed.
        var sql = $""" 
            SELECT id, content
            FROM outbox_messages
            WHERE processed_on_utc IS NULL
            ORDER BY occurred_on_utc
            LIMIT {_outboxOptions.BatchSize}
            FOR UPDATE
            """;
        var outboxMessages = await connection.QueryAsync<OutboxMessageResponse>(sql, transaction: transaction);

        return outboxMessages.ToList();
    }

    private async Task UpdateOutboxMessageAsync(IDbConnection connection, IDbTransaction transaction,
        OutboxMessageResponse outboxMessage, Exception? exception)
    {
        const string sql = @"
            UPDATE outbox_messages
            SET processed_on_utc = @ProcessedOnUtc,
                error = @Error
            WHERE id = @Id";
        await connection.ExecuteAsync(sql,
            new { outboxMessage.Id, ProcessedOnUtc = _dateTimeProvider.UtcNow, Error = exception?.ToString() },
            transaction);
    }
}