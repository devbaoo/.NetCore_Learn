using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Dtos.ExamModel;
using TodoApp.Application.Dtos.ExamResultModel;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures;

namespace ToDoApp.Application.Services;

public interface IExamService
{
    void CreateExam(ExamCreateModel exam);

    void DeleteExam(int id);
    
    ExamViewModel GetExam(int id);
    
    IEnumerable<ExamViewModel> GetExams();
    
    Exam UpdateExam(int id, ExamUpdateModel exam);

     Task<ExamResult> SubmitExam(StudentExamSubmission submission);
}
public class ExamService : IExamService
{
    private readonly IApplicationDBContext _dbContext;
    private readonly IMapper _mapper;
    
    public ExamService(IApplicationDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public void CreateExam(ExamCreateModel exam)
    {
        var data = _mapper.Map<Exam>(exam);
        _dbContext.Exam.Add(data);
        _dbContext.SaveChanges();
    }
    public void DeleteExam(int id)
    {
        var data = _dbContext.Exam.FirstOrDefault(x => x.Id == id);
        if (data == null)
        {
            throw new Exception("Exam not found");
        }
        _dbContext.Exam.Remove(data);
        _dbContext.SaveChanges();
    }
    public ExamViewModel GetExam(int id)
    {
        var exam = _dbContext.Exam.FirstOrDefault(x => x.Id == id);
        if (exam == null)
        {
            return null;
        }
        return _mapper.Map<ExamViewModel>(exam);
    }
    public IEnumerable<ExamViewModel> GetExams()
    {
        var exams = _dbContext.Exam.ToList();
        return _mapper.Map<IEnumerable<ExamViewModel>>(exams);
    }
    public Exam  UpdateExam(int id, ExamUpdateModel exam)
    {
        var data = _dbContext.Exam.FirstOrDefault(x => x.Id == id);
        if (data == null)
        {
            throw new Exception("Exam not found");
        }
        _mapper.Map(exam, data);
        _dbContext.SaveChanges();
        return data;
    }
    public async Task<ExamResult> SubmitExam(StudentExamSubmission submission)
    {
        var exam = await _dbContext.Exam
            .Where(e => e.Id == submission.ExamId)  
            .FirstOrDefaultAsync(); 
        if (exam == null)
        {
            throw new KeyNotFoundException($"Exam with ID {submission.ExamId} not found.");
        }

        var questions = await _dbContext.QuestionBank
            .Where(q => exam.QuestionIds.Contains(q.Id))  
            .ToListAsync();

        if (questions.Count != submission.StudentAnswers.Count)
        {
            throw new ArgumentException("The number of answers provided does not match the number of questions in the exam.");
        }

        int correctAnswers = 0;

        for (int i = 0; i < questions.Count; i++)
        {
            if (questions[i].CorrectAnswer == submission.StudentAnswers[i])
            {
                correctAnswers++;
            }
        }

        decimal score = 10 * (decimal)correctAnswers / questions.Count;

        var examResult = new ExamResult
        {
            StudentId = submission.StudentId,
            CourseId = exam.CourseId,
            Score = score,
            DateCalculated = DateTime.UtcNow
        };

        _dbContext.ExamResult.Add(examResult); 
        _dbContext.SaveChanges();

        return examResult;
    }
}