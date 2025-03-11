using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ToDoApp.Domains.Entities;

namespace ToDoApp.Infrastructures.Interceptors;

public class CourseAuditInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var context = eventData.Context as ApplicationDBContext;
        if (context == null) return base.SavingChanges(eventData, result);

        var currentTime = DateTime.UtcNow;

        foreach (var entry in context.ChangeTracker.Entries<Course>())
        {
            if (entry.State == EntityState.Added)
            {
                var course = entry.Entity;

                course.CreatedAt = currentTime;
                course.CreatedBy = GetCurrentUserId();
                
            }
            if (entry.State == EntityState.Modified)
            {
                var course = entry.Entity;

                course.UpdatedAt = currentTime;
                course.UpdatedBy = GetCurrentUserId(); 
            }
        }

        return base.SavingChanges(eventData, result);
    }

    private int GetCurrentUserId()
    {
        return 1; 
    }
}