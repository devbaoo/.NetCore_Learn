using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Dtos;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures;

namespace ToDoApp.Application.Services;
public interface IStudentService
{
     IEnumerable<StudentViewModel> GetStudents(int? schoolId);
     Student CreateStudent(StudentCreateModel student);
     Student UpdateStudent(int id, StudentUpdateModel student);
     void DeleteStudent(int id);
     Student GetStudent(int id);

     StudentCourseViewModel GetStudentDetail(int id);

     void EnrollCourse(int studentId, int courseId);
}
public class StudentService : IStudentService
{
     private readonly IApplicationDBContext _dbContext;
     
     public StudentService(IApplicationDBContext dbContext)
     {
          _dbContext = dbContext;
     }
     // IQueryable the hien cau query van chua thuc thi
     public IEnumerable<StudentViewModel> GetStudents(int? schoolId)
     {
          // SELECT * FROM Students join Schools on Students.SchoolId = Schools.Id
          var students = _dbContext.Student
               .Include(Student => Student.School)
               .AsQueryable();
          if (schoolId != null)
          { 
               // WHERE School.Id = schoolId
               students = students.Where(x => x.School.Id == schoolId);
          }
          //Select
          //Student.Id
          //Student.FirstName + " " + Student.LastName as FullName
          //Student.Age
          //School.Name as SchoolName
          //From Students
          //join Schools on Students.SchoolId = Schools.Id
          return students.Select(x => new StudentViewModel
          {
               Id = x.Id,
               FullName = x.FirstName + " " + x.LastName,
               Age = x.Age,
               SchoolName = x.School.Name
          }).ToList();
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
          return new StudentCourseViewModel
          {
               StudentId = student.Id,
               StudentName = student.FirstName + " " + student.LastName,
               Courses = student.CourseStudents.Select(x => new CourseViewModel
               {
                    CourseId = x.Course.Id,
                    CourseName  = x.Course.Name
               }).ToList()
          };
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
          


}