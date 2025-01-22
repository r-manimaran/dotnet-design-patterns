using MassTransit;

using NewsLetters.Api.Messages;

namespace NewsLetters.Api.Handlers;
public class OnboardingCompletedHandler(ILogger<OnboardingCompletedHandler> logger) : IConsumer<OnboardingCompleted>
{
    public Task Consume(ConsumeContext<OnboardingCompleted> context)
    {
        logger.LogInformation($"Onboarding completed for {context.Message.Email}");
        
        return Task.CompletedTask;
    }
}
