using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Common;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Dtos;
using ToDoApp.Application.Dtos.StudentModel;
using ToDoApp.Application.Params;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures;

namespace ToDoApp.Application.Services;
public interface IStudentService
{
     Task<PagedResult<StudentViewModel>> GetStudents(StudentQueryParameters query);
     
     Student CreateStudent(StudentCreateModel student);
     Student UpdateStudent(int id, StudentUpdateModel student);
     void DeleteStudent(int id);
     Student GetStudent(int id);

     StudentCourseViewModel GetStudentDetail(int id);

     void EnrollCourse(int studentId, int courseId);

     void UpdateScore(StudentScore model);
     decimal GetStudentAverageScore(int studentId);
}

public class StudentService : IStudentService
{
     private readonly IApplicationDBContext _dbContext;
     private readonly IMapper _mapper;

     public StudentService(IApplicationDBContext dbContext, IMapper mapper)
     {
          _dbContext = dbContext;
          _mapper = mapper;
     }

     // // IQueryable the hien cau query van chua thuc thi
     // public IEnumerable<StudentViewModel> GetStudents(int? schoolId)
     // {
     //      // SELECT * FROM Students join Schools on Students.SchoolId = Schools.Id
     //      var students = _dbContext.Student
     //           .Include(Student => Student.School)
     //           .AsQueryable();
     //      if (schoolId != null)
     //      {
     //           // WHERE School.Id = schoolId
     //           students = students.Where(x => x.School.Id == schoolId);
     //      }
     //      var studentList = students.ToList();
     //      var studentViewModels = _mapper.Map<List<StudentViewModel>>(studentList);
     //
     //      return studentViewModels;
     //      
     //
     //      //Select
     //      //Student.Id
     //      //Student.FirstName + " " + Student.LastName as FullName
     //      //Student.Age
     //      //School.Name as SchoolName
     //      //From Students
     //      //join Schools on Students.SchoolId = Schools.Id
     //      
     // }
     public async Task<PagedResult<StudentViewModel>> GetStudents(StudentQueryParameters query)
     {
          var students = _dbContext.Student
               .Include(s => s.School)
               .AsQueryable();

          // Filter theo school
          if (query.SchoolId.HasValue)
          {
               students = students.Where(s => s.SId == query.SchoolId);
          }

          if (!string.IsNullOrEmpty(query.Keyword))
          {
               string keyword = query.Keyword.ToLower();
               students = students.Where(s =>
                    (s.FirstName + " " + s.LastName).ToLower().Contains(keyword) ||
                    s.School.Name.ToLower().Contains(keyword));
          }

          if (!string.IsNullOrEmpty(query.SortBy))
          {
               students = query.SortDesc
                    ? students.OrderByDescending(s => EF.Property<object>(s, query.SortBy))
                    : students.OrderBy(s => EF.Property<object>(s, query.SortBy));
          }

          // Pagination
          int totalItems = await students.CountAsync();

          var result = await students
               .Skip((query.PageIndex - 1) * query.PageSize)
               .Take(query.PageSize)
               .ProjectTo<StudentViewModel>(_mapper.ConfigurationProvider)
               .ToListAsync();

          return new PagedResult<StudentViewModel>
          {
               TotalItems = totalItems,
               TotalPages = (int)Math.Ceiling((double)totalItems / query.PageSize),
               PageIndex = query.PageIndex,
               PageSize = query.PageSize,
               Items = result
          };
     }
     public Student CreateStudent(StudentCreateModel student)
     {
          var data = new Student
          {
               FirstName = student.FirstName,
               LastName = student.LastName,
               DateOfBirth = student.DateOfBirth,
               Address1 = student.Address1,
               SId = student.SchoolId
          };
          _dbContext.Student.Add(data);
          _dbContext.SaveChanges();
          return data;
     }

     public Student UpdateStudent(int id, StudentUpdateModel student)
     {
          var data = _dbContext.Student.Find(id);
          if (data == null)
          {
               return null;
          }

          data.FirstName = student.FirstName;
          data.LastName = student.LastName;
          data.DateOfBirth = student.DateOfBirth;
          data.Address1 = student.Address1;
          data.balance = student.Balance;
          data.SId = student.SchoolId;
          _dbContext.SaveChanges();
          return data;
     }

     public void DeleteStudent(int id)
     {
          var data = _dbContext.Student.Find(id);
          if (data == null)
          {
               return;
          }

          _dbContext.Student.Remove(data);
          _dbContext.SaveChanges();
     }

     public Student GetStudent(int id)
     {
          return _dbContext.Student.Find(id);
     }

     
     public StudentCourseViewModel GetStudentDetail(int id)
     {
          var student = _dbContext.Student
               .Include(x => x.CourseStudents)
               .ThenInclude(x => x.Course)
               .FirstOrDefault(x => x.Id == id);

          if (student == null)
          {
               return null;
          }

          return _mapper.Map<StudentCourseViewModel>(student);
     }
     

     public void EnrollCourse(int studentId, int courseId)
     {
          var student = _dbContext.Student.Find(studentId);
          if (student == null)
          {
               throw new Exception("Student not found");
          }

          var course = _dbContext.Course.Find(courseId);
          if (course == null)
          {
               throw new Exception("Course not found");
          }

          var courseStudent = new CourseStudent
          {
               StudentId = studentId,
               CourseId = courseId
          };
          _dbContext.CourseStudent.Add(courseStudent);
          _dbContext.SaveChanges();
     }

     public void UpdateScore(StudentScore model)
     {
          var courseStudent = _dbContext.CourseStudent.FirstOrDefault(x => x.StudentId == model.StudentId && x.CourseId == model.CourseId);
          if (courseStudent == null)
          {
               courseStudent = new CourseStudent
               {
                    StudentId = model.StudentId,
                    CourseId = model.CourseId,
                    AssignmentScore = model.AssignmentScore,
                    PracticalScore = model.PracticalScore,
                    FinalScore = model.FinalScore
               };
               _dbContext.CourseStudent.Add(courseStudent);
          }
          else
          {
               courseStudent.AssignmentScore = model.AssignmentScore;
               courseStudent.PracticalScore = model.PracticalScore;
               courseStudent.FinalScore = model.FinalScore;
          }

          _dbContext.SaveChanges();
     }

     public decimal GetStudentAverageScore(int studentId)
     {
          var student = _dbContext.Student
               .Include(x => x.CourseStudents)
               .FirstOrDefault(x => x.Id == studentId);

          if (student == null)
          {
               throw new Exception("Student not found");
          }
          if (student.CourseStudents.Count == 0)
          {
               throw new Exception("Student has not enrolled in any course");
          } 
          //Còn điểm trung bình nhé
          var finalScore = student.CourseStudents.Average(x => x.FinalScore);
          var assignmentScore = student.CourseStudents.Average(x => x.AssignmentScore);
          var practicalScore = student.CourseStudents.Average(x => x.PracticalScore);
          
          var averageScore = (finalScore*3 + assignmentScore + practicalScore*2) / 6;
          return averageScore;
     }   
     
     
      
     
          
}