namespace ToDoApp.Application.Dtos;

public class StudentCourseViewModel
{
    public int StudentId { get; set; }
    public string StudentName { get; set; }

    public List<CourseViewModel> Courses { get; set; }
}