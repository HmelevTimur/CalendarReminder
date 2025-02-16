using CalendarReminder.Domain.Entities;

namespace CalendarReminder.Application.Services.Interfaces;

public interface IPushNotificationService
{
    Task<bool> SendPushNotificationAsync(string deviceToken, string title, string message);
}