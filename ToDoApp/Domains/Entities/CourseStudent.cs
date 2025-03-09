namespace ToDoApp.Domains.Entities;

public class CourseStudent
{
    public int CourseId { get; set; }
    public virtual Course Course { get; set; }
    public int StudentId { get; set; }
    public virtual Student Student { get; set; }
    
    public decimal AssignmentScore { get; set; }
    
    public decimal PracticalScore { get; set; }
    
    public decimal FinalScore { get; set; }
} 