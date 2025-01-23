namespace NewsLetters.Api.Services;

public interface IEmailService
{
    Task<bool> SendWelcomeEmailAsync(string email);
    Task SendFollowUpEmailAsync(string email);
}
