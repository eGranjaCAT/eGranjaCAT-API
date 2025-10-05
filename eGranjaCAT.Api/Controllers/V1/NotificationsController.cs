using Asp.Versioning;
using eGranjaCAT.Api.Extensions;
using eGranjaCAT.Domain.Enums;
using eGranjaCAT.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace eGranjaCAT.Api.Controllers.V1
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }


        [HttpGet()]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = User.GetUserId();
            var result = await _service.GetNotificationsAsync(userId);

            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });
            return Ok(result.Data);
        }

        [HttpGet("high")]
        public async Task<IActionResult> GetHighPriorityNotifications()
        {
            var userId = User.GetUserId();
            var result = await _service.GetNotificationsByPriorityAsync(userId, NotificationPriorityEnum.High);

            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });
            return Ok(result.Data);
        }

        [HttpGet("medium")]
        public async Task<IActionResult> GetMediumPriorityNotifications()
        {
            var userId = User.GetUserId();
            var result = await _service.GetNotificationsByPriorityAsync(userId, NotificationPriorityEnum.Medium);

            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });
            return Ok(result.Data);
        }

        [HttpGet("low")]
        public async Task<IActionResult> GetLowPriorityNotifications()
        {
            var userId = User.GetUserId();
            var result = await _service.GetNotificationsByPriorityAsync(userId, NotificationPriorityEnum.Low);
            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });
            return Ok(result.Data);
        }


        [HttpPost("{notificationId:guid}/read")]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            var userId = User.GetUserId();
            var result = await _service.MarkAsReadAsync(notificationId.ToString());

            if (!result.Success) return StatusCode(result.StatusCode, new { result.Errors });
            return NoContent();
        }
    }
}