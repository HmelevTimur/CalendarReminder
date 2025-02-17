﻿using System.Text.Json.Serialization;

namespace CalendarReminder.Domain.Entities;

public class Reminder
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CalendarEventId { get; set; }
    [JsonIgnore]
    public CalendarEvent? CalendarEvent { get; set; }

    public DateTime ReminderTime { get; set; }

    public string? Message { get; set; }

    public bool IsSent { get; set; }
}