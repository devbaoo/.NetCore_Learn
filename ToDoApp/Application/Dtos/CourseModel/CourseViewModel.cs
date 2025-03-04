namespace ToDoApp.Application.Dtos;

public class CourseViewModel
{
    public int CourseId { get; set; }
    public string CourseName { get; set; }
    
    public decimal AssignmentScore { get; set; }
    
    public decimal PracticalScore { get; set; }
    
    public decimal FinalScore { get; set; }
    public DateTime StartDate { get; set; }
}