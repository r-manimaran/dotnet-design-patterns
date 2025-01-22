using MassTransit;
using NewsLetters.Api.Messages;
using System.Diagnostics;

namespace NewsLetters.Api.Sagas;

public class NewsletterOnboardingSaga : MassTransitStateMachine<NewsletterOnboardingSagaData>
{
    public State Welcoming { get; set; }
    public State FollowingUp { get; set; }
    public State Onboarding { get; set; }

    public Event<SubscriberCreated> SubscriberCreated { get; set; }
    public Event<WelcomeEmailSent> WelcomeEmailSent { get; set; }
    public Event<FollowUpEmailSent> FollowUpEmailSent { get; set; }

    public NewsletterOnboardingSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => SubscriberCreated, x =>
        {
            x.CorrelateById(context => context.Message.SubscriberId);
        });

        Event(() => WelcomeEmailSent, x =>
        {
            x.CorrelateById(context => context.Message.SubscriberId);
        });

        Event(() => FollowUpEmailSent, x =>
        {
            x.CorrelateById(context => context.Message.SubscriberId);
        });

        Initially(
            When(SubscriberCreated)
            .Then(context =>
            {
                // Open Telemetry Activity
                using var activity = Activity.Current?.Source.StartActivity(
                    "Process Subscribe Created",
                    ActivityKind.Consumer);
                activity?.SetTag("CorrelationId", context.Saga.CorrelationId);
                activity?.SetTag("Email", context.Message.Email);

                context.Saga.SubscriberId = context.Message.SubscriberId;
                context.Saga.Email = context.Message.Email;
            })
            .TransitionTo(Welcoming)
            .Publish(context => new SendWelcomeEmail(context.Message.SubscriberId, context.Message.Email)));

        During(Welcoming,
            When(WelcomeEmailSent)
            .Then(context => 
                {
                    using var activity = Activity.Current?.Source.StartActivity(
                        "Process Welcome Email Sent",
                        ActivityKind.Consumer);
                    activity?.SetTag("CorrelationId", context.Saga.CorrelationId);
                    activity?.SetTag("Status", "Email Sent");

                    context.Saga.WelcomeEmailSent = true;
                })
            .TransitionTo(FollowingUp)
            .Publish(context => new SendFollowUpEmail(context.Message.SubscriberId, context.Message.Email)));

        During(FollowingUp,
            When(FollowUpEmailSent)
            .Then(context =>
            {
                using var activity = Activity.Current?.Source.StartActivity(
                    "Process Follow Up Email Sent",
                    ActivityKind.Consumer);
                activity?.SetTag("CorrelationId", context.Saga.CorrelationId);
                activity?.SetTag("Status", "Followup Email Sent");

                context.Saga.FollowUpEmailSent = true;
                context.Saga.OnboardingCompleted = true;
            }).TransitionTo(Onboarding)
            .Publish(context => new OnboardingCompleted
            {                

                SubscriberId = context.Message.SubscriberId,
                Email = context.Message.Email
            })
            .Finalize());

    }
}
