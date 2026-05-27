using FactoryPatternApproach.Models;
using FactoryPatternApproach.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FactoryPatternApproach.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationFactory _notificationFactory;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationFactory notificationFactory,
            ILogger<NotificationController> _logger)
        {
            _notificationFactory = notificationFactory;
            this._logger = _logger;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            try
            {
                var notificationService = _notificationFactory.GetNotificationService(request.Type);
                await notificationService.SendAsync(request);

                return Ok(new
                {
                    Message = $"Notification sent successfully",
                    Type = request.Type,
                    Recipient = request.To
                });

            }
            catch (NotSupportedException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification");
                return StatusCode(500, new { Error = "Internal server error" });
            }
        }


        [HttpGet("supported-types")]
        public IActionResult GetSupportedTypes()
        {
            // Could also expose available types from factory
            return Ok(new[] { "email", "sms", "push" });
        }

    }
}
