
using FluentEmail.Core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace NewsLetters.Api.Services;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IFluentEmail _fluentEmail;

    public EmailService(IFluentEmail fluentEmail, 
                        ILogger<EmailService> logger)
    {
        _fluentEmail = fluentEmail;
        _logger = logger;
    }

    public async Task SendFollowUpEmailAsync(string email)
    {
       _logger.LogInformation($"Sending follow-up email to {email}");
        using var activity = Activity.Current?.Source.StartActivity(
            "Send Follow Up Email",
            ActivityKind.Internal);
        activity?.SetTag("Email", email);
        try
        {
            var response = await _fluentEmail.To(email)
                .Subject("Follow up email")
                .Body("This is a follow-up email", isHtml: true)
                .SendAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error sending follow-up email to {email}");
            throw;
        }

        _logger.LogInformation($"Send follow-up email to {email} successfully");
    }

    public async Task SendWelcomeEmailAsync(string email)
    {
        _logger.LogInformation($"Sending welcome email to {email}");

        var response = await _fluentEmail.To(email)
        .Subject("Welcome email")
        .Body("This is a Welcome email", isHtml: true)
        .SendAsync();
        _logger.LogInformation($"Send Welcome email to {email} successfully");
    }
}
