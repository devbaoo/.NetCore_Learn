﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        
        public DbSet<Exam> Exam { get; set; }
        public DbSet<ExamResult> ExamResult { get; set; }
        public DbSet<Question> QuestionBank { get; set; }
        
        public DbSet<AuditLog> AuditLog { get; set; }
        
        public EntityEntry<T> Entry<T>(T entity) where T : class;
        public int SaveChanges();
    }
}   
