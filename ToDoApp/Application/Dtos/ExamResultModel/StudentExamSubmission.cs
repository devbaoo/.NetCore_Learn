namespace TodoApp.Application.Dtos.ExamResultModel;

public class StudentExamSubmission
{
    public int StudentId { get; set; }
    public int ExamId { get; set; }
    public List<string> StudentAnswers { get; set; }
}   