using AutoMapper;
using TodoApp.Application.Dtos.ExamModel;
using TodoApp.Application.Dtos.ExamResultModel;
using ToDoApp.Domains.Entities;


namespace ToDoApp.Application.MapperProfile;

public class ExamProfile : Profile
{
    public ExamProfile()
    {
        CreateMap<ExamCreateModel, Exam>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Exam, ExamViewModel>()
            .ForMember(dest => dest.QuestionIds, opt => opt.MapFrom(src =>
                src.ExamQuestions.Select(eq => eq.QuestionId).ToList()));
        CreateMap<ExamUpdateModel, Exam>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<StudentExamSubmission, ExamResult>();
    }
}