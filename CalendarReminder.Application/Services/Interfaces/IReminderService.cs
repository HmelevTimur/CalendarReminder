namespace CalendarReminder.Application.Services.Interfaces;

public interface IReminderService
{
    Task CheckAndSendRemindersAsync();
}