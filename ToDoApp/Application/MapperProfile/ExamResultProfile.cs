using AutoMapper;
using TodoApp.Application.Dtos.ExamResultModel;
using ToDoApp.Domains.Entities;

namespace ToDoApp.Application.MapperProfile;

public class ExamResultProfile : Profile
{
    public ExamResultProfile()
    {
        CreateMap<ExamResultCreateModel, ExamResult>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<ExamResult, ExamResultViewModel>();
        CreateMap<ExamResultUpdateModel, ExamResult>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
    }
}