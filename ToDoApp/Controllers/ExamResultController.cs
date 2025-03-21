using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dtos.ExamResultModel;
using ToDoApp.Application.Services;
using ToDoApp.Domains.Entities;

namespace ToDoApp.Controllers;

[Route("[controller]")]
[ApiController]
public class ExamResultController : ControllerBase
{
    public readonly IExamService _examService;
    
    public ExamResultController(IExamService examService)
    {
        _examService = examService;
    }
    
    [HttpPost]
    public async Task<ActionResult<ExamResult>> SubmitExam([FromBody] StudentExamSubmission submission)
    {
        try
        {
            var examResult = await _examService.SubmitExam(submission);
            return Ok(examResult);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(new { status = "error", message = e.Message });
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { status = "error", message = e.Message });
        }
    }
    
}