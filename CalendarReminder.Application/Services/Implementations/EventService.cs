using AutoMapper;
using CalendarReminder.Application.Dtos;
using CalendarReminder.Application.Services.Interfaces;
using CalendarReminder.Domain.Entities;
using CalendarReminder.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CalendarReminder.Application.Services.Implementations;

public class EventService(CalendarDbContext context, IMapper mapper) : IEventService
{
    public async Task<IEnumerable<CalendarEventDto>> GetEventsAsync()
    {
        var events = await context.Events
            .Include(e => e.Reminders)
            .Include(e => e.Users)
            .ToListAsync();

        return mapper.Map<List<CalendarEventDto>>(events);
    }

    public async Task<CalendarEventDto?> GetEventByIdAsync(Guid id)
    {
        var calendarEvent = await context.Events
            .Include(e => e.Reminders)
            .FirstOrDefaultAsync(e => e.Id == id);

        return calendarEvent == null ? null : mapper.Map<CalendarEventDto>(calendarEvent);
    }

    public async Task<CalendarEvent> CreateEventAsync(CalendarEventDto calendarEventDto)
    {
        var userExists = await context.Users.AnyAsync(u => u.Id == calendarEventDto.UserId);
        if (!userExists)
        {
            throw new ArgumentException("Пользователь с таким UserId не найден.");
        }

        var calendarEvent = mapper.Map<CalendarEvent>(calendarEventDto);
        context.Events.Add(calendarEvent);
        await context.SaveChangesAsync();

        return calendarEvent; // Возвращаем объект с Id
    }


    public async Task<bool> UpdateEventAsync(Guid id, CalendarEventDto calendarEventDto)
    {
        var calendarEvent = await context.Events.FindAsync(id);
        if (calendarEvent == null)
            return false;

        mapper.Map(calendarEventDto, calendarEvent);
        context.Events.Update(calendarEvent);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteEventAsync(Guid id)
    {
        var calendarEvent = await context.Events.FindAsync(id);
        if (calendarEvent == null)
            return false;

        context.Events.Remove(calendarEvent);
        await context.SaveChangesAsync();

        return true;
    }
}