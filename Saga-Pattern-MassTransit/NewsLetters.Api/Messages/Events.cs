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

// Failure 
public class WelcomeEmailFailed
{
    public Guid SubscriberId { get; init; }
    public string Email { get; init; }
    public string ErrorMessage  { get; init; }
}

public class FollowUpEmailFailed
{
    public Guid SubscriberId { get; init; }
    public string Email { get; init; }
    public string ErrorMessage  { get; init; }
}

public class OnboardingFailed
{
    public Guid SubscriberId { get; init; }
    public string Email { get; init; }
    public string ErrorMessage  { get; init; }
}

public class NotifyAdminofFailure
{
    public Guid SubscriberId { get; init; }
    public string Email { get; init; }
    public string Stage  { get; init; }
}