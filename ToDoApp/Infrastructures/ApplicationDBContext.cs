﻿using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures.DataMapping;
using ToDoApp.Infrastructures.Interceptors;

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
        
        public DbSet<Exam> Exam { get; set; }
        
        public DbSet<ExamResult> ExamResult { get; set; }
        
        public DbSet<Question> QuestionBank { get; set; }

        
        // Nếu bạn đã cấu hình connection string qua DI, có thể không cần ghi đè OnConfiguring.
        // Nếu cần fallback, hãy sử dụng connection string đúng định dạng:
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // optionsBuilder.UseLazyLoadingProxies();
                optionsBuilder.UseSqlServer("Server=167.99.78.5,1433;Initial Catalog=ToDoApp;Persist Security Info=False;User ID=sa;Password=12345@aA;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
                optionsBuilder.AddInterceptors(new SqlQueryLoggingInterceptor(), new AuditLogInterceptor(), new CourseAuditInterceptor());
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
            modelBuilder.Entity<Student>().HasQueryFilter(s => s.DeletedAt == null);
            modelBuilder.Entity<Course>().HasQueryFilter(c => c.DeletedAt == null);
            base.OnModelCreating(modelBuilder);
        }

        public EntityEntry<T> Entry<T>(T entity) where T : class
        {
            return base.Entry(entity);
        }
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<ISoftDelete>().Where(e => e.State == EntityState.Deleted))
            {
                entry.State = EntityState.Modified;

                entry.Entity.DeletedAt = DateTime.UtcNow;
                entry.Entity.DeletedBy = GetCurrentUserId();
            }

            return base.SaveChanges();
        }
        private int GetCurrentUserId()
        {
            return 1; 
        }
    }
    
}