using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NormalApproachApi.Services;

namespace NormalApproachApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly EmailService _emailService;
    private readonly SmsService _smsService;
    private readonly PushNotificationService _pushNotificationService;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(
        EmailService emailService,
        SmsService smsService,
        PushNotificationService pushNotificationService,
        ILogger<NotificationController> logger)
    {
        _emailService = emailService;
        _smsService = smsService;
        _pushNotificationService = pushNotificationService;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] Model.NotificationRequest request)
    {
        try
        {
            switch (request.Type.ToLower())
            {
                case "email":
                    await _emailService.SendAsync(request);
                    break;
                case "sms":
                    await _smsService.SendAsync(request);
                    break;
                case "push":
                    await _pushNotificationService.SendAsync(request);
                    break;
                default:
                    return BadRequest($"Invalid notification type: {request.Type}");
            }
            return Ok($"Notification sent successfully via {request.Type}.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification");
            return StatusCode(500, "An error occurred while sending the notification.");
        }
    }    
}