using AutoMapper;
using TodoApp.Application.Dtos.QuestionBankModel;
using ToDoApp.Domains.Entities;

namespace ToDoApp.Application.MapperProfile;

public class QuestionBankProfile : Profile
{
    public QuestionBankProfile()
    {
        CreateMap<QuestionCreateModel, Question>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Question, QuestionViewModel>();
        CreateMap<QuestionUpdateModel, Question>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}