using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ToDoApp.Domains.Entities;

namespace ToDoApp.Infrastructures.Interceptors;

public class AuditLogInterceptor : SaveChangesInterceptor
{
    private readonly HashSet<EntityEntry> addSet = new HashSet<EntityEntry>();

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var context = eventData.Context as ApplicationDBContext;
        if (context == null) return base.SavingChanges(eventData, result);

        var auditLogs = new List<AuditLog>();

        foreach (var entity in context.ChangeTracker.Entries())
        {
            if (entity.State == EntityState.Added)
            {
                addSet.Add(entity); // Lưu entity mới để xử lý sau khi lưu thành công
            }

            var log = new AuditLog
            {
                EntityName = entity.Entity.GetType().Name,
                CreatedAt = DateTime.Now,
                Action = entity.State.ToString(),
            };

            if (entity.State == EntityState.Modified)
            {
                log.OldValue = JsonSerializer.Serialize(entity.OriginalValues.ToObject());
                log.NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject());
            }
            else if (entity.State == EntityState.Deleted)
            {
                log.OldValue = JsonSerializer.Serialize(entity.OriginalValues.ToObject());
            }

            auditLogs.Add(log);
        }

        if (auditLogs.Any())
        {
            context.AuditLog.AddRange(auditLogs);
        }

        return base.SavingChanges(eventData, result);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        var context = eventData.Context as ApplicationDBContext;
        if (context == null) return base.SavedChanges(eventData, result);

        if (addSet.Any())
        {
            var auditLogs = addSet.Select(entity => new AuditLog
            {
                EntityName = entity.Entity.GetType().Name,
                CreatedAt = DateTime.Now,
                Action = "Added",
                NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject())
            }).ToList();

            context.AuditLog.AddRange(auditLogs);
            addSet.Clear();
        }

        return base.SavedChanges(eventData, result);
    }
}