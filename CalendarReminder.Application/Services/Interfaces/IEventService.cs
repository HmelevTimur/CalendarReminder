using CalendarReminder.Application.Dtos;
using CalendarReminder.Domain.Entities;

namespace CalendarReminder.Application.Services.Interfaces;

public interface IEventService
{
    Task<IEnumerable<CalendarEventDto>> GetEventsAsync();
    Task<CalendarEventDto?> GetEventByIdAsync(Guid id);
    Task<CalendarEvent> CreateEventAsync(CalendarEventDto calendarEventDto);
    Task<bool> UpdateEventAsync(Guid id, CalendarEventDto calendarEventDto);
    Task<bool> DeleteEventAsync(Guid id);
}