namespace TodoApp.Application.Dtos.ExamModel;

public class ExamViewModel
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public List<string> QuestionIds { get; set; }
    public DateTime ExamDate { get; set; }
    public int TotalQuestions => QuestionIds.Count;
    public int TimeLimitInMinutes { get; set; }  
    public string Name { get; set; }  
}