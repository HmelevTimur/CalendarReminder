using CalendarReminder.Infrastructure.Persistence;
using CalendarReminder.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalendarReminder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController(CalendarDbContext context) : ControllerBase
    {
        // Получение всех событий
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetEvents()
        {
            var events = await context.Events
                .Include(e => e.Reminders)
                .Include(e => e.Users)
                .ToListAsync();
            return Ok(events);
        }

        // Получение одного события по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarEvent>> GetEvent(Guid id)
        {
            var calendarEvent = await context.Events
                .Include(e => e.Reminders)
                .Include(e => e.Users) 
                .FirstOrDefaultAsync(e => e.Id == id);

            if (calendarEvent == null)
                return NotFound("Event not found.");

            return Ok(calendarEvent);
        }

        // Создание нового события
        [HttpPost]
        public async Task<ActionResult<CalendarEvent>> CreateEvent(CalendarEvent calendarEvent)
        {
            // Связываем пользователя с событием
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == calendarEvent.CreatedBy.Id);
            if (user == null) return BadRequest("User not found.");

            calendarEvent.CreatedBy = user;

            context.Events.Add(calendarEvent);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = calendarEvent.Id }, calendarEvent);
        }

        // Обновление события
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, CalendarEvent calendarEvent)
        {
            if (id != calendarEvent.Id)
                return BadRequest("Event ID mismatch.");

            context.Entry(calendarEvent).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        // Удаление события
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var calendarEvent = await context.Events.FindAsync(id);
            if (calendarEvent == null)
                return NotFound("Event not found.");

            context.Events.Remove(calendarEvent);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
