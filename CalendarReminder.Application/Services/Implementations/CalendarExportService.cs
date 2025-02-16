using System.Globalization;
using System.Text;
using AutoMapper;
using CalendarReminder.Application.Dtos;
using CalendarReminder.Application.Services.Interfaces;
using CalendarReminder.Infrastructure.Persistence;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CalendarReminder.Application.Services.Implementations;

public class CalendarExportService(CalendarDbContext context, IMapper mapper) : ICalendarExportService
{
    public async Task<byte[]> ExportCalendarToCsvAsync(Guid userId)
    {
        var events = await context.Events
            .Where(e => e.UserId == userId)
            .ToListAsync();

        if (!events.Any())
            throw new InvalidOperationException("У пользователя нет событий для экспорта.");

        var eventDtos = mapper.Map<List<CalendarEventCsvDto>>(events);

        using var memoryStream = new MemoryStream();
        await using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
        await using var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = ";"
        });

        await csvWriter.WriteRecordsAsync(eventDtos);
        await streamWriter.FlushAsync();
        
        return memoryStream.ToArray();
    }
}