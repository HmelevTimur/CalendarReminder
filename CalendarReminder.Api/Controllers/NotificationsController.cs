using CalendarReminder.Infrastructure.Persistence;
using CalendarReminder.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalendarReminder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(CalendarDbContext context) : ControllerBase
    {
        // Отправка уведомлений для напоминаний
        [HttpPost("send/{reminderId}")]
        public async Task<IActionResult> SendNotification(Guid reminderId)
        {
            var reminder = await context.Reminders
                .Include(r => r.CalendarEvent)
                .Include(r => r.CalendarEvent.Users)
                .FirstOrDefaultAsync(r => r.Id == reminderId);

            if (reminder == null)
                return NotFound("Reminder not found.");

            // Проверяем, если напоминание уже было отправлено
            if (reminder.IsSent)
                return BadRequest("Reminder already sent.");

            // Логика отправки уведомлений (например, через Firebase или SignalR)
            foreach (var user in reminder.CalendarEvent.Users)
            {
                await SendPushNotification(user, reminder.Message);
            }

            // Отмечаем напоминание как отправленное
            reminder.IsSent = true;
            context.Reminders.Update(reminder);
            await context.SaveChangesAsync();

            return Ok("Notifications sent.");
        }

        private Task SendPushNotification(User user, string? message)
        {
            // Пример: Псевдокод для отправки уведомлений
            // Реализуйте логику через SignalR, Firebase, или иной сервис
            return Task.CompletedTask;
        }
    }
}