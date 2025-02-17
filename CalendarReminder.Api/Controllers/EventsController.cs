using CalendarReminder.Application.Dtos;
using CalendarReminder.Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace CalendarReminder.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController(IEventService eventService) : ControllerBase
{
    [HttpGet]
    [EnableQuery]
    public async Task<ActionResult<IEnumerable<CalendarEventDto>>> GetEvents()
    {
        var events = await eventService.GetEventsAsync();
        return Ok(events);
    }

    [HttpGet("{id}")]
    [EnableQuery]
    public async Task<IActionResult> GetEvent(Guid id)
    {
        var calendarEvent = await eventService.GetEventByIdAsync(id);
        if (calendarEvent == null)
            return NotFound();

        return Ok(calendarEvent);
    }


    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CalendarEventDto calendarEventDto)
    {
        try
        {
            var createdEvent = await eventService.CreateEventAsync(calendarEventDto);
            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(Guid id, [FromBody] CalendarEventDto calendarEventDto)
    {
        var updated = await eventService.UpdateEventAsync(id, calendarEventDto);
        return updated ? NoContent() : NotFound("Событие не найдено.");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(Guid id)
    {
        var deleted = await eventService.DeleteEventAsync(id);
        return deleted ? NoContent() : NotFound("Событие не найдено.");
    }
}