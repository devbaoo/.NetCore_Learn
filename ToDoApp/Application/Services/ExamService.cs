using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Common;
using TodoApp.Application.Dtos.ExamModel;
using TodoApp.Application.Dtos.ExamResultModel;
using ToDoApp.Application.Params;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures;

namespace ToDoApp.Application.Services;

public interface IExamService
{
    void CreateExam(ExamCreateModel exam);

    void DeleteExam(int id);
    
    ExamViewModel GetExam(int id);

    Task<PagedResult<ExamViewModel>> GetExams(ExamQueryParameters query);
    
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
    
    public void CreateExam(ExamCreateModel model)
    {
        var exam = _mapper.Map<Exam>(model);

        exam.ExamQuestions = model.QuestionIds.Select(qId => new ExamQuestion
        {
            QuestionId = qId
        }).ToList();

        _dbContext.Exam.Add(exam);
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
        var exam = _dbContext.Exam
            .Include(e => e.ExamQuestions)
            .ThenInclude(eq => eq.Question)
            .FirstOrDefault(x => x.Id == id);

        if (exam == null)
        {
            return null;
        }

        return _mapper.Map<ExamViewModel>(exam);
    }

    public async Task<PagedResult<ExamViewModel>> GetExams(ExamQueryParameters query)
    {
        var exams = _dbContext.Exam
            .Include(e => e.ExamQuestions)
            .ThenInclude(eq => eq.Question)
            .AsQueryable();

        // 🔍 Filter theo keyword
        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            exams = exams.Where(e => e.Name.ToLower().Contains(query.Keyword.ToLower()));
        }

        if (!string.IsNullOrEmpty(query.SortBy))
        {
            exams = query.SortDesc
                ? exams.OrderByDescending(e => EF.Property<object>(e, query.SortBy))
                : exams.OrderBy(e => EF.Property<object>(e, query.SortBy));
        }

        var totalItems = await exams.CountAsync();

        var result = await exams
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ProjectTo<ExamViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<ExamViewModel>
        {
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / query.PageSize),
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            Items = result
        };
    }

    public Exam UpdateExam(int id, ExamUpdateModel model)
    {
        var exam = _dbContext.Exam
            .Include(e => e.ExamQuestions)
            .FirstOrDefault(x => x.Id == id);

        if (exam == null)
        {
            throw new Exception("Exam not found");
        }

        _mapper.Map(model, exam);

        // Cập nhật danh sách câu hỏi
        exam.ExamQuestions.Clear(); // Xóa câu hỏi cũ
        foreach (var qId in model.QuestionIds)
        {
            exam.ExamQuestions.Add(new ExamQuestion
            {
                ExamId = exam.Id,
                QuestionId = qId
            });
        }

        _dbContext.SaveChanges();
        return exam;
    }

    public async Task<ExamResult> SubmitExam(StudentExamSubmission submission)
    {
        var student = _dbContext.Student.FirstOrDefault( s => s.Id == submission.StudentId);
        if (student == null)
        {
            throw new KeyNotFoundException($"Student with ID {submission.StudentId} not found.");
        }
        // Lấy bài thi
        var exam = await _dbContext.Exam
            .Include(e => e.ExamQuestions)
            .ThenInclude(eq => eq.Question)
            .FirstOrDefaultAsync(e => e.Id == submission.ExamId);

        if (exam == null)
        {
            throw new KeyNotFoundException($"Exam with ID {submission.ExamId} not found.");
        }

        // Lấy danh sách câu hỏi từ bảng trung gian ExamQuestion
        var questions = exam.ExamQuestions.Select(eq => eq.Question).ToList();

        if (questions.Count != submission.StudentAnswers.Count)
        {
            throw new ArgumentException("The number of answers provided does not match the number of questions in the exam.");
        }

        // Chấm điểm
        int correctAnswers = 0;

        for (int i = 0; i < questions.Count; i++)
        {
            if (questions[i].CorrectAnswer == submission.StudentAnswers[i])
            {
                correctAnswers++;
            }
        }

        decimal score = 10 * (decimal)correctAnswers / questions.Count;

        // Lưu kết quả
        var examResult = new ExamResult
        {
            StudentId = submission.StudentId,
            ExamId = submission.ExamId,
            Score = score,
            DateCalculated = DateTime.UtcNow
        };

        _dbContext.ExamResult.Add(examResult); 
        _dbContext.SaveChanges();

        return examResult;
    }

}