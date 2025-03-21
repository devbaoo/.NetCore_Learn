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
        CreateMap<Exam, ExamViewModel>();
        CreateMap<ExamUpdateModel, Exam>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<StudentExamSubmission, ExamResult>();
    }
}