using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Common;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Dtos;
using ToDoApp.Application.Params;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures;

namespace ToDoApp.Application.Services;
public interface ISchoolService
{
    Task<PagedResult<SchoolViewModel>> GetSchools(SchoolQueryParameters query);
    
    School CreateSchool(SchoolCreateModel school);
    
    School UpdateSchool(int id, SchoolUpdateModel school);
    
    void DeleteSchool(int id);
    
    School GetSchool(int id);

    SchoolStudentViewModel GetSchoolDetail(int schoolId);
    // SchoolStudentViewModel GetSchoolDetail(int schoolId);
}

public class SchoolService : ISchoolService
{
    private readonly IApplicationDBContext _dbContext;

    public SchoolService(IApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<SchoolViewModel>> GetSchools(SchoolQueryParameters query)
    {
        var schools = _dbContext.School.AsQueryable();

        // 🔍 Filter
        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var keyword = query.Keyword.ToLower();
            schools = schools.Where(x =>
                x.Name.ToLower().Contains(keyword) ||
                x.Address.ToLower().Contains(keyword));
        }

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            schools = query.SortDesc
                ? schools.OrderByDescending(s => EF.Property<object>(s, query.SortBy))
                : schools.OrderBy(s => EF.Property<object>(s, query.SortBy));
        }

        var totalItems = await schools.CountAsync();
        var items = await schools
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new SchoolViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Address = x.Address
            })
            .ToListAsync();

        return new PagedResult<SchoolViewModel>
        {
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / query.PageSize),
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            Items = items
        };
    }

    public School CreateSchool(SchoolCreateModel school)
    {
        var data = new School
        {
            Name = school.Name,
            Address = school.Address
        };
        _dbContext.School.Add(data);
        _dbContext.SaveChanges();
        return data;
    }

    public School UpdateSchool(int id, SchoolUpdateModel school)
    {
        var data = _dbContext.School.Find(id);
        if (data == null)
        {
            throw new Exception("School not found");
        }

        data.Name = school.Name;
        data.Address = school.Address;
        _dbContext.SaveChanges();
        return data;
    }

    public void DeleteSchool(int id)
    {
        var data = _dbContext.School.Find(id);
        if (data == null)
        {
            throw new Exception("School not found");
        }

        _dbContext.School.Remove(data);
        _dbContext.SaveChanges();
    }

    public School GetSchool(int id)
    {
        var data = _dbContext.School.Find(id);
        if (data == null)
        {
            throw new Exception("School not found");
        }

        return data;
    }

    public SchoolStudentViewModel GetSchoolDetail(int schoolId)
    {
        var school = _dbContext.School.Find(schoolId);
        if (school == null)
        {
            return null;
        }
        
        _dbContext.Entry(school).Collection(s => s.Students).Load();
        
        var student = school.Students;


        return new SchoolStudentViewModel
        {
            Id = school.Id,
            Name = school.Name,
            Address = school.Address,
            Students = student.Select(x => new StudentViewModel
            {
                Id = x.Id,
                FullName = x.FirstName + " " + x.LastName,
                Age = x.Age,
                SchoolName = x.School.Name,
            }).ToList()
        };

    }

}