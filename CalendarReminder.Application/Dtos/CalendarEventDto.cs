namespace CalendarReminder.Application.Dtos;

public class CalendarEventDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventTime { get; set; }
    public Guid UserId { get; set; }
    public List<ReminderDto> Reminders { get; set; } = new();
}