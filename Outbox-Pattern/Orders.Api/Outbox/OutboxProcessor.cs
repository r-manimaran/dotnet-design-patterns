using Dapper;
using MassTransit;
using Npgsql;
using System.Text.Json;
using Messaging.Contracts;
namespace Orders.Api.Outbox;

internal sealed class OutboxProcessor(
            NpgsqlDataSource dataSource,
            IPublishEndpoint publishEndpoint)
{
    private const int BatchSize = 10;
    public async Task<int> Execute(CancellationToken cancellationToken)
    {
        using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken);
        const string sql = """
            SELECT *
            FROM outbox_messages
            WHERE processed_on_utc is NULL
            ORDER BY occured_on_utc LIMIT @BatchSize
            """;
        var outboxMessages = (await connection.QueryAsync<OutboxMessage>(
                sql,
                new {BatchSize},
                transaction:transaction)).AsList();
        foreach (var outboxMessage in outboxMessages) 
        {
            try
            {
                var messageType = Messaging.Contracts.AssemblyReference.Assembly.GetType(outboxMessage.Type);

                var deserializedMessage = JsonSerializer.Deserialize(outboxMessage.Content, messageType);

                await publishEndpoint.Publish(deserializedMessage, messageType, cancellationToken);

                const string sql_update = """ 
                    UPDATE outbox_messages
                    SET processed_on_utc = @ProcessedOnUtc
                    Where id =@Id
                    """;

                await connection.ExecuteAsync(
                   sql_update,
                   new { ProcessedOnUtc = DateTime.UtcNow, outboxMessage.Id },
                   transaction: transaction);

            }
            catch (Exception ex) 
            {
                const string sql_update_onerror = """ 
                    UPDATE outbox_messages
                    SET processed_on_utc = @ProcessedOnUtc, error =@Error
                    Where id =@Id
                    """;

                await connection.ExecuteAsync(
                   sql_update_onerror,
                   new 
                   { 
                       ProcessedOnUtc = DateTime.UtcNow, 
                       Error =ex.ToString(),
                       outboxMessage.Id 
                   },
                   transaction: transaction);

                Console.WriteLine(ex);
                throw;
            }
        }

        await  transaction.CommitAsync(cancellationToken);
        return outboxMessages.Count;
    }
}
