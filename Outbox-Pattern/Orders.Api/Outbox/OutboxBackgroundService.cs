
namespace Orders.Api.Outbox;

public class OutboxBackgroundService(
                    IServiceScopeFactory serviceScopeFactory,
                    ILogger<OutboxBackgroundService> logger) :BackgroundService

{
    private const int OutboxProcessorFrequency = 7;
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {

            logger.LogInformation("Starting OutboxBackgroundService...");
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = serviceScopeFactory.CreateScope();
                var outboxProcessor = scope.ServiceProvider.GetRequiredService<OutboxProcessor>();

                await outboxProcessor.Execute(stoppingToken);

                await Task.Delay(TimeSpan.FromSeconds(OutboxProcessorFrequency), stoppingToken);
            }
        }
        catch (OperationCanceledException)
        {
            logger.LogInformation("OutboxBackgroundService cancelled.");
        }
        catch (Exception ex)
        {
            logger.LogInformation($"Failed to execute {ex}");
        }
        finally
        {
            logger.LogInformation("OutboxBackgroundService finished...");
        }
    }
}
