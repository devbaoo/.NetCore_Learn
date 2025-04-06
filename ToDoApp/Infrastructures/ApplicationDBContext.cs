using System.Text.Json;
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
                optionsBuilder.UseSqlServer("Server=db-devbaoo.cj604kkc40w8.ap-southeast-1.rds.amazonaws.com,1433;Initial Catalog=ToDoApp;Persist Security Info=False;User ID=admin;Password=Khacbao0712;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");
                optionsBuilder.AddInterceptors(new SqlQueryLoggingInterceptor(), new AuditLogInterceptor(), new CourseAuditInterceptor());
            }
            
        }

protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    // Existing relationship configurations
    modelBuilder.Entity<Student>()
        .Property(x => x.Age)
        .HasComputedColumnSql("DATEDIFF(YEAR, DateOfBirth, GETDATE())");
        
    // Student - CourseStudent relationship
    modelBuilder.Entity<Student>()
        .HasMany(x => x.CourseStudents)
        .WithOne(x => x.Student)
        .HasForeignKey(x => x.StudentId);
        
    // Course - CourseStudent relationship
    modelBuilder.Entity<Course>()
        .HasMany(x => x.CourseStudents)
        .WithOne(x => x.Course)
        .HasForeignKey(x => x.CourseId);
        
    // Student - School relationship
    modelBuilder.Entity<Student>()
        .HasOne(s => s.School)
        .WithMany(sch => sch.Students)
        .HasForeignKey(s => s.SId);
        
    // CourseStudent composite key
    modelBuilder.Entity<CourseStudent>()
        .HasKey(x => new {x.CourseId, x.StudentId});
        
    // New relationship configurations
    
    // Exam - Course relationship (assuming an Exam belongs to a Course)
    modelBuilder.Entity<Exam>()
        .HasOne(e => e.Course)
        .WithMany(c => c.Exams)
        .HasForeignKey(e => e.CourseId);
        
    // Exam - Question relationship (assuming many-to-many through a join table)
    modelBuilder.Entity<ExamQuestion>()
        .HasKey(eq => new { eq.ExamId, eq.QuestionId });

    modelBuilder.Entity<ExamQuestion>()
        .HasOne(eq => eq.Exam)
        .WithMany(e => e.ExamQuestions)
        .HasForeignKey(eq => eq.ExamId);

    modelBuilder.Entity<ExamQuestion>()
        .HasOne(eq => eq.Question)
        .WithMany(q => q.ExamQuestions)
        .HasForeignKey(eq => eq.QuestionId);

        
    // ExamResult relationships
    modelBuilder.Entity<ExamResult>()
        .HasOne(er => er.Exam)
        .WithMany(e => e.ExamResult)
        .HasForeignKey(er => er.ExamId);

    modelBuilder.Entity<ExamResult>()
        .HasOne(er => er.Student)
        .WithMany(s => s.ExamResult)
        .HasForeignKey(er => er.StudentId);

    
    modelBuilder.ApplyConfiguration(new CourseMapping());
    
    modelBuilder.Entity<Student>().HasQueryFilter(s => s.DeletedAt == null);
    modelBuilder.Entity<Course>().HasQueryFilter(c => c.DeletedAt == null);
    modelBuilder.Entity<Exam>().HasQueryFilter(e => e.DeletedAt == null);
    modelBuilder.Entity<Question>().HasQueryFilter(q => q.DeletedAt == null);
    modelBuilder.Entity<School>().HasQueryFilter(s => s.DeletedAt == null);
    
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