using AutoMapper;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Dtos;
using ToDoApp.Domains.Entities;

namespace ToDoApp.Application.MapperProfile;

public class TodoProfile : Profile
{
    public TodoProfile()
    {
        //Map course to CourseViewModel
        CreateMap<Course, CourseViewModels>()
            .ForMember(dest => dest.CourseId, x => x.MapFrom(src => src.Id))
            .ForMember(dest => dest.CourseName, x => x.MapFrom(src => src.Name));
        CreateMap<CourseCreateModel, Course>()
            .ForMember(dest => dest.Name, x => x.MapFrom(src => src.CourseName));
        CreateMap<CourseUpdateModel, Course>()
            .ForMember(dest => dest.Name, x => x.MapFrom(src => src.CourseName));
        CreateMap<CourseViewModels, Course>()
            .ForMember(dest => dest.Id, x => x.MapFrom(src => src.CourseId))
            .ForMember(dest => dest.Name, x =>
            {
                x.PreCondition(src => src.CourseName != null);
                x.MapFrom(src => src.CourseName);
            });
        CreateMap<Student, StudentViewModel>()
            .ForMember(dest => dest.Id, x => x.MapFrom(src => src.Id))
            .ForMember(dest => dest.FullName, x => x.MapFrom(src => src.FirstName + " " + src.LastName))
            .ForMember(dest => dest.SchoolName, x => x.MapFrom(src => src.School.Name));
        CreateMap<Course, CourseStudentViewModel>()
            .ForMember(dest => dest.CourseId, x => x.MapFrom(src => src.Id))
            .ForMember(dest => dest.CourseName, x => x.MapFrom(src => src.Name))
            .ForMember(dest => dest.Students, x => x.MapFrom(src => src.CourseStudents.Select(x => new StudentViewModel

            {
                FullName = x.Student.FirstName + " " + x.Student.LastName,
                Age = x.Student.Age,
                SchoolName = x.Student.School.Name
            })));
        CreateMap<CourseStudent, CourseViewModel>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Course.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Course.Name))
            .ForMember(dest => dest.AssignmentScore, opt => opt.MapFrom(src => src.AssignmentScore))
            .ForMember(dest => dest.PracticalScore, opt => opt.MapFrom(src => src.PracticalScore))
            .ForMember(dest => dest.FinalScore, opt => opt.MapFrom(src => src.FinalScore));
        CreateMap<Student, StudentCourseViewModel>()
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.FirstName + " " + src.LastName))
            .ForMember(dest => dest.Courses, opt => opt.MapFrom(src => src.CourseStudents));
    }
}