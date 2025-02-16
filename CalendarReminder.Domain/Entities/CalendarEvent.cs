using System.ComponentModel.DataAnnotations;

namespace CalendarReminder.Domain.Entities;

public class CalendarEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required] public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime EventTime { get; set; }
    public Guid UserId { get; set; }  
    public User? CreatedBy { get; set; } 
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
}