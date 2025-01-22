using Dapper;
using MassTransit;
using Messaging.Contracts;
using Npgsql;

namespace Shipping.Api.Shipments
{
    internal sealed class OrderCreatedIntegrationEventConsumer(
        ILogger<OrderCreatedIntegrationEventConsumer> logger,
        NpgsqlDataSource datasource) : IConsumer<OrderCreatedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
        {
            var orderId = context.Message.OrderId;
            logger.LogInformation("Processing order {orderId}", orderId);

            var shipment = new Shipment 
            { 
                Id = Guid.NewGuid(),
                OrderId = orderId,
                Status = ShipmentStatus.Pending.ToString(),
                CreatedAt = DateTime.UtcNow,
            };

            const string sql =
                """
                    INSERT INTO shipments (id, order_id, status, created_at, updated_at)
                    VALUES (@Id,@OrderId, @Status, @CreatedAt, @UpdatedAt);
                """;
            using var connection = await datasource.OpenConnectionAsync();
            await connection.ExecuteAsync(sql, shipment);

            logger.LogInformation("Shipment created for order {OrderId}", orderId);
        }
    }
}
