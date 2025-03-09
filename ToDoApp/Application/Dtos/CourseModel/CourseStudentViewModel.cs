using TodoApp.Application.Dtos;

namespace ToDoApp.Application.Dtos;

public class CourseStudentViewModel
{
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    public DateTime StartDate { get; set; }
    
    public  List<StudentViewModel> Students { get; set; }
    
}