using TodoApp.Application.Dtos;

namespace ToDoApp.Application.Dtos;

public class SchoolStudentViewModel
{
    public int Id  { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    
    public  IEnumerable<StudentViewModel> Students { get; set; }
}