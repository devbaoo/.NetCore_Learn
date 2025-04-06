using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Common;
using TodoApp.Application.Dtos.QuestionBankModel;
using ToDoApp.Application.Params;
using ToDoApp.Application.Services;

namespace ToDoApp.Controllers;

[Route("[controller]")]
[ApiController]
public class QuestionBankController : ControllerBase
{
    private readonly IQuestionBankService _questionBankService;
    public QuestionBankController(IQuestionBankService questionBankService)
    {
        _questionBankService = questionBankService;
    }
    
    [HttpPost]
    public ActionResult CreateQuestion([FromBody] QuestionCreateModel question)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        var createdQuestion = _questionBankService.CreateQuestion(question);
        return Ok(createdQuestion);
    }
    [HttpDelete("{id}")]
    public ActionResult DeleteQuestion(int id)
    {
        try
        {
            _questionBankService.DeleteQuestion(id);
            return Ok(new { status = "success", message = "Question deleted" });
        }
        catch (Exception e)
        {
            return NotFound(new { status = "error", message = e.Message });
        }
    }
    [HttpGet("{id}")]
    public ActionResult GetQuestion(int id)
    {
        var question = _questionBankService.GetQuestion(id);
        if (question == null)
        {
            return NotFound(new { status = "error", message = "Question not found" });
        }
        return Ok(question);
    }
    [HttpGet]
    public async Task<ActionResult<PagedResult<QuestionViewModel>>> GetQuestions([FromQuery] QuestionQueryParameters query)
    {
        var result = await _questionBankService.GetQuestions(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public ActionResult UpdateQuestion(int id, [FromBody] QuestionUpdateModel question)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        try
        {
            var updatedQuestion = _questionBankService.UpdateQuestion(id, question);
            return Ok(updatedQuestion);
        }
        catch (Exception e)
        {
            return NotFound(new { status = "error", message = e.Message });
        }
    }
}