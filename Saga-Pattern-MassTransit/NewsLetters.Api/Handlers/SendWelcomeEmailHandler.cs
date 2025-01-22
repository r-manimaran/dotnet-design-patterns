using MassTransit;
using NewsLetters.Api.Messages;
using NewsLetters.Api.Services;

namespace NewsLetters.Api.Handlers;

public class SendWelcomeEmailHandler(IEmailService emailService) : IConsumer<SendWelcomeEmail>
{
    public async Task Consume(ConsumeContext<SendWelcomeEmail> context)
    {
        await emailService.SendWelcomeEmailAsync(context.Message.Email);

        await context.Publish(new WelcomeEmailSent
        {
            SubscriberId = context.Message.SubscriberId,
            Email = context.Message.Email
        });
    }
}
