namespace ToDoApp.Domains.Entities;

public class Exam : ISoftDelete
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; }

    public ICollection<ExamQuestion> ExamQuestions { get; set; } = new List<ExamQuestion>();
    public DateTime ExamDate { get; set; }
    public int TimeLimitInMinutes { get; set; }  
    public string Name { get; set; }
    public virtual ICollection<ExamResult> ExamResult { get; set; } = new List<ExamResult>();

    public int? DeletedBy { get; set; }  
    public DateTime? DeletedAt { get; set; }
}

