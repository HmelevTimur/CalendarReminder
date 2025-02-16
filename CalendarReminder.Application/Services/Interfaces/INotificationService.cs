using CalendarReminder.Domain.Entities;

namespace CalendarReminder.Application.Services.Interfaces;

public interface INotificationService
{
    Task SendPushNotificationAsync(string deviceToken, string message);

}