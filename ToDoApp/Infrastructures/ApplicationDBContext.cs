using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures.DataMapping;

namespace ToDoApp.Infrastructures
{
    public class ApplicationDBContext : DbContext, IApplicationDBContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<ToDo> ToDos { get; set; }
        
        public DbSet<Student> Student { get; set; }
        
        public DbSet<School> School { get; set; }
        
        public DbSet<Course> Course { get; set; }
        
        public DbSet<AuditLog> AuditLog { get; set; }

        
        public DbSet<CourseStudent> CourseStudent { get; set; }

        
        // Nếu bạn đã cấu hình connection string qua DI, có thể không cần ghi đè OnConfiguring.
        // Nếu cần fallback, hãy sử dụng connection string đúng định dạng:
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseSqlServer("Server=167.99.78.5,1433;Initial Catalog=ToDoApp;Persist Security Info=False;User ID=sa;Password=12345@aA;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .Property(x => x.Age)
                .HasComputedColumnSql("DATEDIFF(YEAR, DateOfBirth, GETDATE())");
            modelBuilder.Entity<Student>()
                .HasMany(x => x.CourseStudents)
                .WithOne(x => x.Student)
                .HasForeignKey(x => x.StudentId);
            modelBuilder.Entity<Course>()
                .HasMany(x => x.CourseStudents)
                .WithOne(x => x.Course)
                .HasForeignKey(x => x.CourseId);
            modelBuilder.Entity<Student>()
                .HasOne(s => s.School)
                .WithMany(sch => sch.Students)
                .HasForeignKey(s => s.SId);
            modelBuilder.Entity<CourseStudent>()
                .HasKey(x => new {x.CourseId, x.StudentId});
            modelBuilder.ApplyConfiguration(new CourseMapping());
            base.OnModelCreating(modelBuilder);
        }

        public EntityEntry<T> Entry<T>(T entity) where T : class
        {
            return base.Entry(entity);
        }
        public int SaveChanges()
        {
            var auditLogs = new List<AuditLog>();
            foreach (var entity in ChangeTracker.Entries()) {
                var log = new AuditLog
                {
                    EntityName = entity.Entity.GetType().Name,
                    CreatedAt = DateTime.Now,
                    Action = entity.State.ToString(),
                
                };
                if (entity.State == EntityState.Added)
                {
                    log.NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject());
                }
                if (entity.State == EntityState.Modified)
                {
                    log.OldValue = JsonSerializer.Serialize(entity.OriginalValues.ToObject());
                    log.NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject());
                }
                if (entity.State == EntityState.Deleted)
                {
                    log.OldValue = JsonSerializer.Serialize(entity.OriginalValues.ToObject());
                }
                auditLogs.Add(log);

            }
            AuditLog.AddRange(auditLogs); //state 
            return base.SaveChanges();
        }
    }
    
}