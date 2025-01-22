using MassTransit;
using Microsoft.AspNetCore.Components;
using NewsLetters.Api.Database;
using NewsLetters.Api.Messages;
using NewsLetters.Api.OpenTelemetry;
using System.Diagnostics;

namespace NewsLetters.Api.Handlers;

public class SubscribeToNewsletterHandler(AppDbContext dbContext) : IConsumer<SubscribeToNewsletter>
{
    public async Task Consume(ConsumeContext<SubscribeToNewsletter> context)
    {

        using var activity = Activity.Current?.Source.StartActivity(
            $"Consume {nameof(SubscribeToNewsletter)}",
            ActivityKind.Consumer);
        
        activity?.SetTag("MessageId", context.MessageId);
        activity?.SetTag("Email", context.Message.Email);

      
        var subscriber = dbContext.Subscribers.Add(new Subscriber
        {
            Id = Guid.NewGuid(),
            Email = context.Message.Email,
            SubscribedOnUtc = DateTime.UtcNow
        });

        await dbContext.SaveChangesAsync();

        await context.Publish(new SubscriberCreated
        {
            SubscriberId = subscriber.Entity.Id,
            Email = subscriber.Entity.Email
        });
    }
}

