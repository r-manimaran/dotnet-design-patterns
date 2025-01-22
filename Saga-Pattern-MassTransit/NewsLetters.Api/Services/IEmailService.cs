namespace NewsLetters.Api.Services;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email);
    Task SendFollowUpEmailAsync(string email);
}
