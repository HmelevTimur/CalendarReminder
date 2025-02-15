using CalendarReminder.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace CalendarReminder.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalendarController : ControllerBase
{
    private readonly CalendarDbContext _context;
    public CalendarController(CalendarDbContext context) => _context = context;

    [HttpGet]
    [EnableQuery]
    public IActionResult GetEvents() => Ok(_context.Events);

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] CalendarEvent calendarEvent)
    {
        _context.Events.Add(calendarEvent);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetEvents), new { id = calendarEvent.Id }, calendarEvent);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEvent(int id, [FromBody] CalendarEvent updatedEvent)
    {
        if (id != updatedEvent.Id) return BadRequest();
        _context.Entry(updatedEvent).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var calendarEvent = await _context.Events.FindAsync(id);
        if (calendarEvent == null) return NotFound();
        _context.Events.Remove(calendarEvent);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}