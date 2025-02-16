using CalendarReminder.Application.Dtos;
using CalendarReminder.Application.Services.Implementations;
using Microsoft.AspNetCore.Mvc;

namespace CalendarReminder.Api.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationController(FirebaseNotificationService firebaseService) : ControllerBase
{
    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequestDto request)
    {
        var response = await firebaseService.SendPushNotificationAsync(request.DeviceToken, request.Title, request.Body);
        
        if (response != null)
            return Ok(new { MessageId = response });

        return BadRequest("Ошибка при отправке уведомления");
    }
}