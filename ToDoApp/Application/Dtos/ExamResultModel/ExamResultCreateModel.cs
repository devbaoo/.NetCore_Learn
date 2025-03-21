namespace TodoApp.Application.Dtos.ExamResultModel;

public class ExamResultCreateModel
{
    public int CourseId { get; set; }
    public int StudentId { get; set; }
    public decimal Score { get; set; }
    public DateTime DateCalculated { get; set; }
}