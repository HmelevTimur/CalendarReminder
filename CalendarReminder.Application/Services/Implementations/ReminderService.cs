using CalendarReminder.Application.Services.Interfaces;
using CalendarReminder.Domain.Entities;
using CalendarReminder.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CalendarReminder.Application.Services.Implementations;

public class ReminderService(CalendarDbContext context, IPushNotificationService pushNotificationService) : IReminderService
{
    public async Task CheckAndSendRemindersAsync()
    {
        var now = DateTime.UtcNow;

        var reminders = await context.Reminders
            .Where(r => r.ReminderTime <= now && !r.IsSent)
            .Include(r => r.CalendarEvent)
            .Include(r => r.CalendarEvent!.CreatedBy) 
            .ToListAsync();

        foreach (var reminder in reminders)
        {
            if (reminder.CalendarEvent?.CreatedBy == null || string.IsNullOrEmpty(reminder.CalendarEvent.CreatedBy.DeviceToken))
                continue;

            var deviceToken = reminder.CalendarEvent.CreatedBy.DeviceToken; 

            var result = await pushNotificationService.SendPushNotificationAsync(
                deviceToken,
                $"Напоминание: {reminder.CalendarEvent.Title}",
                reminder.Message ?? "У вас запланировано событие."
            );

            if (result != null)
            {
                reminder.IsSent = true;
            }
        }

        await context.SaveChangesAsync();
    }
}