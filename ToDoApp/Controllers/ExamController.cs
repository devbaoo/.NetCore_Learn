using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Common;
using TodoApp.Application.Dtos.ExamModel;
using ToDoApp.Application.Params;
using ToDoApp.Application.Services;

namespace ToDoApp.Controllers;

[Route("[controller]")]
[ApiController]
public class ExamController : ControllerBase
{
    private readonly IExamService _examService;
    public ExamController(IExamService examService)
    {
        _examService = examService;
    }
    
    [HttpPost]
    public ActionResult CreateExam([FromBody] ExamCreateModel exam)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        _examService.CreateExam(exam);
        return Ok(exam);
    }
    
    [HttpDelete("{id}")]
    public ActionResult DeleteExam(int id)
    {
        try
        {
            _examService.DeleteExam(id);
            return Ok(new { status = "success", message = "Exam deleted" });
        }
        catch (Exception e)
        {
            return NotFound(new { status = "error", message = e.Message });
        }
    }
    [HttpGet("{id}")]
    public ActionResult GetExam(int id)
    {
        var exam = _examService.GetExam(id);
        if (exam == null)
        {
            return NotFound(new { status = "error", message = "Exam not found" });
        }
        return Ok(exam);
    }
    
    [HttpGet]
    public async Task<ActionResult<PagedResult<ExamViewModel>>> GetExams([FromQuery] ExamQueryParameters query)
    {
        var result = await _examService.GetExams(query);
        return Ok(result);
    }

    
    [HttpPut("{id}")]
    public ActionResult UpdateExam(int id, [FromBody] ExamUpdateModel exam)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        
        var updatedExam = _examService.UpdateExam(id, exam);
        return Ok(updatedExam);
    }
    
}