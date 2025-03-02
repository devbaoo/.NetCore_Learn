using Microsoft.EntityFrameworkCore;
using ToDoApp.Domains.Entities;

namespace ToDoApp.Infrastructures
{
    public interface IApplicationDBContext
    {
        public DbSet<ToDo> ToDos { get; set; }
        public DbSet<Student> Student { get; set; }
        
        public DbSet<School> School { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseStudent> CourseStudent { get; set; }

        public int SaveChanges();
    }
}
