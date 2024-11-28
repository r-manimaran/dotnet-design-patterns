namespace Shipping.Api.Shipments
{
    internal sealed class OrderCreatedIntegrationEventConsumer(
        ILogger<OrderCreatedIntegrationEventConsumer> logger,
        NpgsqlDataSource datasource) : IConsumer<OrderCreatedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
        {

        }
    }
}
