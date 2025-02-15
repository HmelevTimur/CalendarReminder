using System.ComponentModel.DataAnnotations;

namespace CalendarReminder.Domain.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required] public string UserName { get; set; }
    public ICollection<CalendarEvent> CalendarEvents { get; set; }
}