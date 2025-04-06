namespace ToDoApp.Application.Dtos;

public class StudentCreateModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    public string Address1 { get; set; }
    
    public int SchoolId { get; set; }
}