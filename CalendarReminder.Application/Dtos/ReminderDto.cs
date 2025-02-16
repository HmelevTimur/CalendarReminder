namespace CalendarReminder.Application.Dtos;

public class ReminderDto
{
    public DateTime ReminderTime { get; set; }
    public string Message { get; set; } = string.Empty;
}