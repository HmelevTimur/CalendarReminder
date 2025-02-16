namespace CalendarReminder.Application.Dtos;

public class NotificationRequestDto
{
    public string DeviceToken { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
}