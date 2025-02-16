using CalendarReminder.Application.MappingProfile;
using CalendarReminder.Application.Services.Implementations;
using CalendarReminder.Application.Services.Interfaces;
using CalendarReminder.Infrastructure.Persistence;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<CalendarDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddControllers().AddOData(options =>
    options.Select().Filter().OrderBy().Expand().SetMaxTop(100));

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICalendarExportService, CalendarExportService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IPushNotificationService, TestNotificationService>();
builder.Services.AddScoped<IReminderService, ReminderService>();
builder.Services.AddHostedService<ReminderBackgroundService>();



builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CalendarReminder API V1");
        c.RoutePrefix = string.Empty;
    });
}


app.UseAuthorization();
app.MapControllers();

app.Run();
