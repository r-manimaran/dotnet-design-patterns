using MassTransit;
using NewsLetters.Api.Messages;
using NewsLetters.Api.Services;
using System.Diagnostics;

namespace NewsLetters.Api.Handlers;

public class SendWelcomeEmailHandler(IEmailService emailService) : IConsumer<SendWelcomeEmail>
{
    public async Task Consume(ConsumeContext<SendWelcomeEmail> context)
    {
        using var activity = Activity.Current?.Source.StartActivity(
            $"Consume {nameof(SendWelcomeEmail)}",
            ActivityKind.Consumer);

        activity?.SetTag("MessageId", context.MessageId);
        activity?.SetTag("Email", context.Message.Email);
        activity?.SetTag("SubscriberId", context.Message.SubscriberId);
        try
        {
            bool isSuccess = await emailService.SendWelcomeEmailAsync(context.Message.Email);
             // Failed to send the Email
            if(!isSuccess)
            {
                activity?.SetTag("Error", "Failed to send welcome email");
                activity?.SetStatus(ActivityStatusCode.Error);

                await context.Publish(new WelcomeEmailFailed
                {
                    SubscriberId = context.Message.SubscriberId,
                    Email = context.Message.Email,
                    ErrorMessage = "Failed to send welcome email"
                });

                return;

            }

            activity?.SetStatus(ActivityStatusCode.Ok);
            await context.Publish(new WelcomeEmailSent
            {
                SubscriberId = context.Message.SubscriberId,
                Email = context.Message.Email
            });
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);

            throw;
        }
    }
}
