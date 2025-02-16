using CalendarReminder.Application.Dtos;
using CalendarReminder.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CalendarReminder.Api.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController(IPushNotificationService pushNotificationService) : ControllerBase
    {
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequestDto request)
        {
            var response = await pushNotificationService.SendPushNotificationAsync(request.DeviceToken, request.Title, request.Body);
            
            if (response)
                return Ok(new { Message = "Уведомление успешно отправлено" });

            return BadRequest("Ошибка при отправке уведомления");
        }
    }
}