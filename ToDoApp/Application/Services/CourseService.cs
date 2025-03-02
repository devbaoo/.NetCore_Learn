using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Dtos;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures;

namespace ToDoApp.Application.Services;


public interface ICourseService
{
    Course CreateCourse(CourseCreateModel course);
    void DeleteCourse(int id);
    
    Course UpdateCourse(int id, CourseUpdateModel course);
    
    Course GetCourse(int id);
    
    IEnumerable<CourseViewModel> GetCourses();

    CourseStudentViewModel GetCourseDetail(int id);
}

public class CourseService : ICourseService
{
    private readonly IApplicationDBContext _dbContext;
     
    public CourseService(IApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public Course CreateCourse(CourseCreateModel course)
    {
        var data = new Course
        {
            Name = course.CourseName,
            StartDate = course.StartDate
        };
        _dbContext.Course.Add(data);
        _dbContext.SaveChanges();
        
        return data;
    }
    
    public void DeleteCourse(int id)
    {
        var data = _dbContext.Course.Find(id);
        if (data == null)
        {
            throw new Exception("Course not found");
        }
        _dbContext.Course.Remove(data);
        _dbContext.SaveChanges();
    }
    public Course UpdateCourse(int id, CourseUpdateModel course)
    {
        var data = _dbContext.Course.Find(id);
        if (data == null)
        {
            throw new Exception("Course not found");
        }
        data.Name = course.CourseName;
        data.StartDate = course.StartDate;
        _dbContext.SaveChanges();
        return data;
    }
    public Course GetCourse(int id)
    {
        var data = _dbContext.Course.Find(id);
        if (data == null)
        {
            throw new Exception("Course not found");
        }
        return data;
    }
    public IEnumerable<CourseViewModel> GetCourses()
    {
        return _dbContext.Course.Select(x => new CourseViewModel
        {
            CourseId = x.Id,
            CourseName = x.Name,
            StartDate = x.StartDate
        }).ToList();
    }
    
    public CourseStudentViewModel GetCourseDetail(int id)
    {
        var course = _dbContext.Course
            .Include(x => x.CourseStudents)
            .ThenInclude(x => x.Student)
            .FirstOrDefault(x => x.Id == id);
        if (course == null)
        {
            return null;
        }
        return new CourseStudentViewModel
        {
            CourseId = course.Id,
            CourseName = course.Name,
            Students = course.CourseStudents.Select(x => new StudentViewModel
            {
                FullName  = x.Student.FirstName + " " + x.Student.LastName,
                Age = x.Student.Age,
            }).ToList()
        };
    }
    
    
    
    
}