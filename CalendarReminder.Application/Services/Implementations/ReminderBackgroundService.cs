using CalendarReminder.Application.Services.Interfaces;

namespace CalendarReminder.Application.Services.Implementations;

public class ReminderBackgroundService(IServiceProvider serviceProvider, ILogger<ReminderBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("ReminderBackgroundService запущен.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var reminderService = scope.ServiceProvider.GetRequiredService<IReminderService>();

                await reminderService.CheckAndSendRemindersAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Ошибка при отправке напоминаний.");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}