namespace NewsLetters.Api.Messages;

public class SubscriberCreated
{
    public Guid SubscriberId { get; init; }
    public string Email { get; init; }
}

public class WelcomeEmailSent
{
    public Guid SubscriberId { get; init; }
    public string Email { get; init; }
}

public class FollowUpEmailSent
{
    public Guid SubscriberId { get; init; }
    public string Email { get; init; }
}
public class OnboardingCompleted
{
    public Guid SubscriberId { get; init; }
    public string Email { get; init; }
}
