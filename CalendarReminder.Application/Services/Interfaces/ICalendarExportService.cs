namespace CalendarReminder.Application.Services.Interfaces;

public interface ICalendarExportService
{
    Task<byte[]> ExportCalendarToCsvAsync(Guid userId);
}