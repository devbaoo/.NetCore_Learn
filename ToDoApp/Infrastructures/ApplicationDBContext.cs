using Microsoft.EntityFrameworkCore;
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
        
        public DbSet<CourseStudent> CourseStudent { get; set; }

        
        // Nếu bạn đã cấu hình connection string qua DI, có thể không cần ghi đè OnConfiguring.
        // Nếu cần fallback, hãy sử dụng connection string đúng định dạng:
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
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
            modelBuilder.Entity<CourseStudent>()
                .HasKey(x => new {x.CourseId, x.StudentId});
            modelBuilder.ApplyConfiguration(new CourseMapping());
            base.OnModelCreating(modelBuilder);
        }

        public int SaveChanges()
        {
            return base.SaveChanges();
        }
    }
}