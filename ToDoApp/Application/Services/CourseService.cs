using AutoMapper;
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
    
    IEnumerable<CourseViewModels> GetCourses();

    CourseStudentViewModel GetCourseDetail(int id);
}

public class CourseService : ICourseService
{
    private readonly IApplicationDBContext _dbContext;
    private readonly IMapper _mapper;
    
    public CourseService(IApplicationDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public Course CreateCourse(CourseCreateModel course)
    {
        var data = _mapper.Map<Course>(course);
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
        _mapper.Map(course, data);
        _dbContext.SaveChanges();
        return data;
    }
    public Course GetCourse(int id)
    {
        var data = _dbContext.Course.FirstOrDefault(x => x.Id == id);
        if (data == null)
        {
            throw new Exception("Course not found");
        }
        return data;
    } 
    public IEnumerable<CourseViewModels> GetCourses()
{
    var  query =  _dbContext.Course.ToList();
    // return _mapper.Map<IEnumerable<CourseViewModel>>(query);
            
    var result = _mapper.ProjectTo<CourseViewModels>(query.AsQueryable()).ToList();
    return result;
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
        return _mapper.Map<CourseStudentViewModel>(course);
    }

    
}