namespace ToDoApp.Application.Dtos;

public class StudentUpdateModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }  
    public DateTime DateOfBirth { get; set; }
    
    public decimal Balance { get; set; }
    public string Address1 { get; set; }
    public int SchoolId { get; set; }
}