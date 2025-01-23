using MassTransit;
using NewsLetters.Api.Messages;
using System.Diagnostics;

namespace NewsLetters.Api.Sagas;

public class NewsletterOnboardingSaga : MassTransitStateMachine<NewsletterOnboardingSagaData>
{
    public State Welcoming { get; set; }
    public State FollowingUp { get; set; }
    public State Onboarding { get; set; }

    // Failure States
    public State WelcomingFailed { get; set; }
    public State FollowingUpFailed { get; set; }
    public State Faulted {get;set;}


    // Events
    public Event<SubscriberCreated> SubscriberCreated { get; private set; } = null!;
    public Event<WelcomeEmailSent> WelcomeEmailSent { get; private set; } = null!;
    public Event<FollowUpEmailSent> FollowUpEmailSent { get; private set; } = null!;

    // Failure Events
    public Event<WelcomeEmailFailed> WelcomeEmailFailed { get; private set; } = null!;
    public Event<FollowUpEmailFailed> FollowUpEmailFailed { get; private set; } = null!;
    public Event<OnboardingFailed> OnboardingFailed { get; private set; } = null!;

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

        // Define Failure Events
        Event(() => WelcomeEmailFailed, x =>
        {
            x.CorrelateById(context => context.Message.SubscriberId);
        });

        Event(() => FollowUpEmailFailed, x =>
        {
            x.CorrelateById(context => context.Message.SubscriberId);
        });

        Event(() => OnboardingFailed, x =>
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

                    context.Saga.WelcomeEmailSent = false;
                })
            .TransitionTo(FollowingUp)
            .Publish(context => new SendFollowUpEmail(context.Message.SubscriberId, context.Message.Email)),
          When(WelcomeEmailFailed)
            .Then(context =>
            {
                using var activity = Activity.Current?.Source.StartActivity(
                    "Process Welcome Email Failed",
                    ActivityKind.Consumer);
                activity?.SetTag("CorrelationId", context.Saga.CorrelationId);
                activity?.SetTag("Status", "Email Failed");

                context.Saga.WelcomeEmailSent = false;
                context.Saga.IsCompensating = true;
                context.Saga.LastErrorMessages = context.Message.ErrorMessage;
                context.Saga.LastFailureTime = DateTime.UtcNow;
                context.Saga.RetryCount++;
            })
            .TransitionTo(WelcomingFailed)
            .Publish(context => new OnboardingFailed
            {
                SubscriberId = context.Message.SubscriberId,
                Email = context.Message.Email
            })
            .Finalize());

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
            }),
            When(FollowUpEmailFailed)
            .Then(context =>
            {
                using var activity = Activity.Current?.Source.StartActivity(
                    "Process Follow Up Email Failed",
                    ActivityKind.Consumer);
                activity?.SetTag("CorrelationId", context.Saga.CorrelationId);
                activity?.SetTag("Status", "Followup Email Failed");

                context.Saga.FollowUpEmailSent = false;
            })
            .TransitionTo(FollowingUpFailed)
            .Publish(context => new OnboardingFailed
            {
                SubscriberId = context.Message.SubscriberId,
                Email = context.Message.Email
            })
            .Finalize());
        
        DuringAny(
            When(OnboardingFailed)
            .Then(context =>
            {
                using var activity = Activity.Current?.Source.StartActivity(
                    "Process Onboarding Failed",
                    ActivityKind.Consumer);
                activity?.SetTag("CorrelationId", context.Saga.CorrelationId);
                activity?.SetTag("Status", "Onboarding Failed");

                context.Saga.OnboardingCompleted = false;
                context.Saga.LastErrorMessages = context.Message.ErrorMessage;
                context.Saga.IsCompensating  = true;
            })
            .TransitionTo(Faulted));

            SetCompletedWhenFinalized();

    }
}
