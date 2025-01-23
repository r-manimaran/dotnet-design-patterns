using MassTransit;
using Microsoft.EntityFrameworkCore;
using NewsLetters.Api.Database;
using NewsLetters.Api.Messages;
using System.Diagnostics;

namespace NewsLetters.Api.Handlers;

public class OnboardingFailedHandler(AppDbContext dbContext, ILogger<OnboardingFailedHandler> logger) : IConsumer<OnboardingFailed>
{
    public async Task Consume(ConsumeContext<OnboardingFailed> context)
    {
        using var activity = Activity.Current?.Source.StartActivity(
                            $"Consume {nameof(OnboardingFailed)}",
                            ActivityKind.Consumer);

        try
        {
            activity?.SetTag("MessageId", context.MessageId);
            activity?.SetTag("SubscriberId", context.Message.SubscriberId);
            activity?.SetTag("Email", context.Message.Email);
            activity?.SetTag("ErrorMessage", context.Message.ErrorMessage);

            logger.LogError(
                "Onboarding failed for subscriber {SubscriberId} ({Email}). Error: {ErrorMessage}",
                context.Message.SubscriberId,
                context.Message.Email,
                context.Message.ErrorMessage);

            var subscriber = await dbContext.Subscribers
                .FirstOrDefaultAsync(s => s.Id == context.Message.SubscriberId);

            if(subscriber !=null)
            {
                activity?.SetTag("Action", "Removing subscriber");

                dbContext.Subscribers.Remove(subscriber);
                await dbContext.SaveChangesAsync();

                 activity?.SetStatus(ActivityStatusCode.Ok);
                
                logger.LogInformation(
                    "Subscriber {SubscriberId} ({Email}) removed due to onboarding failure",
                    context.Message.SubscriberId,
                    context.Message.Email);
            }
            else
            {
                 activity?.SetTag("Action", "Subscriber not found");
                activity?.SetStatus(ActivityStatusCode.Error);
                
                logger.LogWarning(
                    "Subscriber {SubscriberId} not found for removal after onboarding failure",
                    context.Message.SubscriberId);
            }
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);

            logger.LogError(ex, $"Onboarding failed for {context.Message.Email}. Error: {context.Message.ErrorMessage}");
            throw;
        }
        
    }
}



