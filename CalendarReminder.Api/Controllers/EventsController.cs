using AutoMapper;
using CalendarReminder.Application.Dtos;
using CalendarReminder.Infrastructure.Persistence;
using CalendarReminder.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalendarReminder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController(CalendarDbContext context, IMapper mapper) : ControllerBase
    {
        // Получение всех событий
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CalendarEventDto>>> GetEvents()
        {
            var events = await context.Events
                .Include(e => e.Reminders)
                .Include(e => e.Users)
                .ToListAsync();

            var eventDtos = mapper.Map<List<CalendarEventDto>>(events);
            return Ok(eventDtos);
        }

        // Получение одного события по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<CalendarEventDto>> GetEvent(Guid id)
        {
            var calendarEvent = await context.Events
                .Include(e => e.Reminders)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (calendarEvent == null)
                return NotFound("Событие не найдено.");

            var eventDto = mapper.Map<CalendarEventDto>(calendarEvent);
            return Ok(eventDto);
        }

        // Создание нового события
        [HttpPost]
        public async Task<IActionResult> CreateEvent([FromBody] CalendarEventDto? calendarEventDto)
        {
            if (calendarEventDto == null)
            {
                return BadRequest("Переданы неверные данные.");
            }

            if (calendarEventDto.UserId == null)
            {
                return BadRequest("Пользователь не найден.");
            }

            var userExists = await context.Users.AnyAsync(u => u.Id == calendarEventDto.UserId);
            if (!userExists)
            {
                return BadRequest("Пользователь не найден.");
            }

            var calendarEvent = mapper.Map<CalendarEvent>(calendarEventDto);

            context.Events.Add(calendarEvent);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvent), new { id = calendarEvent.Id },
                mapper.Map<CalendarEventDto>(calendarEvent));
        }

        // Обновление события
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] CalendarEventDto calendarEventDto)
        {
            var calendarEvent = await context.Events.FindAsync(id);
            if (calendarEvent == null)
                return NotFound("Событие не найдено.");

            mapper.Map(calendarEventDto, calendarEvent);

            context.Events.Update(calendarEvent);
            await context.SaveChangesAsync();

            return NoContent();
        }

        // Удаление события
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(Guid id)
        {
            var calendarEvent = await context.Events.FindAsync(id);
            if (calendarEvent == null)
                return NotFound("Событие не найдено.");

            context.Events.Remove(calendarEvent);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
