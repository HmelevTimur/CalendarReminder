using CalendarReminder.Application.Services.Interfaces;

namespace CalendarReminder.Application.Services.Implementations;

public class TestNotificationService : IPushNotificationService
{
    public Task<bool> SendPushNotificationAsync(string deviceToken, string title, string message)
    {
        Console.WriteLine($"Отправка уведомления: {title} - {message}");
        return Task.FromResult(true);  
    }
}