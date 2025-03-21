using AutoMapper;
using TodoApp.Application.Dtos.QuestionBankModel;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures;

namespace ToDoApp.Application.Services;

public interface IQuestionBankService
{
    Question CreateQuestion(QuestionCreateModel question);

    void DeleteQuestion(int id);
    QuestionViewModel GetQuestion(int id);
    IEnumerable<QuestionViewModel> GetQuestions();

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
    public IEnumerable<QuestionViewModel> GetQuestions()
    {
        var questions = _dbContext.QuestionBank.ToList();
        return _mapper.Map<IEnumerable<QuestionViewModel>>(questions);
        
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