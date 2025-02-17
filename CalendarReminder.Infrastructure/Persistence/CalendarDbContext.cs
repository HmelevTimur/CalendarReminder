using CalendarReminder.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalendarReminder.Infrastructure.Persistence;

public class CalendarDbContext(DbContextOptions<CalendarDbContext> options) : DbContext(options)
{
    public DbSet<CalendarEvent> Events { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Reminder> Reminders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CalendarEvent>()
            .HasOne(ce => ce.CreatedBy)
            .WithMany(u => u.CalendarEvents)
            .HasForeignKey(ce => ce.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}