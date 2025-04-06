namespace TodoApp.Application.Dtos.ExamModel;

public class ExamCreateModel
{
    public int CourseId { get; set; }
    public string Name { get; set; }
    public DateTime ExamDate { get; set; }
    public int TimeLimitInMinutes { get; set; }
    public List<int> QuestionIds { get; set; }
}
