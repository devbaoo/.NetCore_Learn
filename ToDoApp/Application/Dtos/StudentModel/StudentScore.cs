namespace ToDoApp.Application.Dtos.StudentModel;

public class StudentScore
{
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public decimal AssignmentScore { get; set; }
    public decimal PracticalScore { get; set; }
    public decimal FinalScore { get; set; }
}