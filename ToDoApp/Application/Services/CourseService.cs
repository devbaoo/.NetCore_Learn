using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Common;
using ToDoApp.Application.Dtos;
using ToDoApp.Application.Params;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures;

namespace ToDoApp.Application.Services;


public interface ICourseService
{
    Course CreateCourse(CourseCreateModel course);
    void DeleteCourse(int id);
    
    Course UpdateCourse(int id, CourseUpdateModel course);
    
     Task<PagedResult<CourseViewModels>> GetCourses(CourseQueryParameters query);
    
    CourseStudentViewModel GetCourseDetail(int id);
    Course GetCourse(int id);
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
    public Course GetCourse(int id)
    {
        var data = _dbContext.Course.Find(id);
        if (data == null)
        {
            throw new Exception("Course not found");
        }
        return data;
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
    public async Task<PagedResult<CourseViewModels>> GetCourses(CourseQueryParameters query)
    {
        var courses = _dbContext.Course.AsQueryable();

        // Optional: Filter by keyword (nếu bạn muốn)
        // if (!string.IsNullOrEmpty(query.Keyword))
        // {
        //     courses = courses.Where(c => c.Name.Contains(query.Keyword));
        // }

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            courses = query.SortDesc
                ? courses.OrderByDescending(c => EF.Property<object>(c, query.SortBy))
                : courses.OrderBy(c => EF.Property<object>(c, query.SortBy));
        }
        if (!string.IsNullOrEmpty(query.Keyword))
        {
            courses = courses.Where(c =>
                c.Name.ToLower().Contains(query.Keyword.ToLower()));
        }
        
        int totalItems = await courses.CountAsync();

        var items = await courses
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ProjectTo<CourseViewModels>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<CourseViewModels>
        {
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / query.PageSize),
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            Items = items
        };
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