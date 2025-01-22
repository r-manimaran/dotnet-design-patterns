namespace NewsLetters.Api.Messages;

public record SendWelcomeEmail(Guid SubscriberId, string Email);

public record SendFollowUpEmail(Guid SubscriberId, string Email);

public record SubscribeToNewsletter(string Email);