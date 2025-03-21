using ToDoApp.Application.MapperProfile;
using TodoApp.Application.Services;
using ToDoApp.Application.Services;
using ToDoApp.Infrastructures;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<IApplicationDBContext, ApplicationDBContext>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IGuidGenerator, GuidGenerator>();  
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IQuestionBankService, QuestionBankService>();
builder.Services.AddScoped<IExamService, ExamService >();
//
builder.Services.AddAutoMapper(typeof(TodoProfile));
builder.Services.AddAutoMapper(typeof(QuestionBankProfile));
builder.Services.AddAutoMapper(typeof(ExamProfile));

// DI Containers

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
