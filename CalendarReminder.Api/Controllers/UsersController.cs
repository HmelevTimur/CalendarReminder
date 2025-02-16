using AutoMapper;
using CalendarReminder.Application.Dtos;
using CalendarReminder.Application.Services.Interfaces;
using CalendarReminder.Domain.Entities;
using CalendarReminder.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CalendarReminder.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(
        CalendarDbContext context,
        IMapper mapper,
        ICalendarExportService calendarExportService) : ControllerBase
    {
        // Получение всех пользователей
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnUserDto>>> GetUsers()
        {
            var users = await context.Users.ToListAsync();
            var userDtos = mapper.Map<List<ReturnUserDto>>(users);
            return Ok(userDtos);
        }

        // Получение пользователя по ID
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnUserDto>> GetUser(Guid id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return NotFound("Пользователь не найден.");

            var userDto = mapper.Map<ReturnUserDto>(user);
            return Ok(userDto);
        }

        // Создание нового пользователя
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.UserName))
                return BadRequest("Имя пользователя не может быть пустым.");

            var user = mapper.Map<User>(userDto);
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var returnUserDto = mapper.Map<ReturnUserDto>(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, returnUserDto);
        }

        [HttpGet("export/{userId}")]
        public async Task<IActionResult> ExportCalendar(Guid userId)
        {
            try
            {
                var csvData = await calendarExportService.ExportCalendarToCsvAsync(userId);
                return File(csvData, "text/csv", $"calendar.csv");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}