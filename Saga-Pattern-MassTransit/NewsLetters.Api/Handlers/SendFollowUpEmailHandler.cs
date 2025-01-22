using MassTransit;
using NewsLetters.Api.Messages;
using NewsLetters.Api.Services;

namespace NewsLetters.Api.Handlers;

public class SendFollowUpEmailHandler(IEmailService emailService) : IConsumer<SendFollowUpEmail>
{
    public async Task Consume(ConsumeContext<SendFollowUpEmail> context)
    {
        await emailService.SendFollowUpEmailAsync(context.Message.Email);

        await context.Publish(new FollowUpEmailSent
        { 
            SubscriberId = context.Message.SubscriberId,
            Email = context.Message.Email
        });
    }
}
