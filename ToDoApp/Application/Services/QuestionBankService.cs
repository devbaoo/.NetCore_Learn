using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Common;
using TodoApp.Application.Dtos.QuestionBankModel;
using ToDoApp.Application.Params;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures;

namespace ToDoApp.Application.Services;

public interface IQuestionBankService
{
    Question CreateQuestion(QuestionCreateModel question);

    void DeleteQuestion(int id);
    QuestionViewModel GetQuestion(int id);
    Task<PagedResult<QuestionViewModel>> GetQuestions(QuestionQueryParameters query);

    Question UpdateQuestion(int id, QuestionUpdateModel question);

}
public class QuestionBankService: IQuestionBankService
{
    private readonly IApplicationDBContext _dbContext;
    private readonly IMapper _mapper;
    
    public QuestionBankService(IApplicationDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public Question CreateQuestion(QuestionCreateModel question)
    {
        var data = _mapper.Map<Question>(question);
        _dbContext.QuestionBank.Add(data);
        _dbContext.SaveChanges();
        
        return data;
    }
    
    public void DeleteQuestion(int id)
    {
        var data = _dbContext.QuestionBank.FirstOrDefault(q => q.Id == id);
        if (data == null)
        {
            throw new Exception("Question not found");
        }
        _dbContext.QuestionBank.Remove(data);
        _dbContext.SaveChanges();
    }
    public QuestionViewModel GetQuestion(int id)
    {
        var question = _dbContext.QuestionBank.FirstOrDefault(q => q.Id == id);
        if (question == null)
        {
            return null;
        }
        return _mapper.Map<QuestionViewModel>(question);
       
    }
    public async Task<PagedResult<QuestionViewModel>> GetQuestions(QuestionQueryParameters query)
    {
        var questions = _dbContext.QuestionBank.AsQueryable();

        // 🔍 Filter theo nội dung câu hỏi
        if (!string.IsNullOrWhiteSpace(query.Keyword))
        {
            var keyword = query.Keyword.ToLower();
            questions = questions.Where(q => q.QuestionText.ToLower().Contains(keyword));
        }

        // 🔃 Sắp xếp động
        if (!string.IsNullOrEmpty(query.SortBy))
        {
            questions = query.SortDesc
                ? questions.OrderByDescending(q => EF.Property<object>(q, query.SortBy))
                : questions.OrderBy(q => EF.Property<object>(q, query.SortBy));
        }

        // 📊 Pagination
        int totalItems = await questions.CountAsync();
        var items = await questions
            .Skip((query.PageIndex - 1) * query.PageSize)
            .Take(query.PageSize)
            .ProjectTo<QuestionViewModel>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PagedResult<QuestionViewModel>
        {
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling((double)totalItems / query.PageSize),
            PageIndex = query.PageIndex,
            PageSize = query.PageSize,
            Items = items
        };
    }
    public Question UpdateQuestion(int id, QuestionUpdateModel question)
    {
        var data = _dbContext.QuestionBank.FirstOrDefault(q => q.Id == id); 

        if (data == null)
        {
            throw new KeyNotFoundException($"Question with ID {id} not found.");
        }

        _mapper.Map(question, data);
        _dbContext.SaveChanges();
        return data;
    }

}