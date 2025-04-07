using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Common;
using ToDoApp.Application.Dtos;
using ToDoApp.Application.Params;
using ToDoApp.Application.Services;
using ToDoApp.Domains.Entities;

namespace ToDoApp.Controllers;

[Route("[controller]")]
[ApiController]
public class SchoolController : ControllerBase
{
    private readonly ISchoolService _schoolService;
    
    public SchoolController(ISchoolService schoolService)
    {
        _schoolService = schoolService;
    }
    
    [HttpGet]
    [HttpGet]
    public async Task<ActionResult<PagedResult<SchoolViewModel>>> GetSchools([FromQuery] SchoolQueryParameters query)
    {
        var result = await _schoolService.GetSchools(query);
        return Ok(result);
    }

    
    [HttpGet("{id}")]
    public ActionResult<School> GetSchool(int id)
    {
        var school = _schoolService.GetSchool(id);
        if (school == null)
        {
            return NotFound(new { status = "error", message = "School not found" });
        }
        return Ok(school);
    }
    
    [HttpPost]
    public ActionResult<School> CreateSchool([FromBody] SchoolCreateModel school)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        var createdSchool = _schoolService.CreateSchool(school);
        return Ok(createdSchool);
    }
    
    [HttpPut("{id}")]
    public ActionResult<School> UpdateSchool(int id, [FromBody] SchoolUpdateModel school)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        var updatedSchool = _schoolService.UpdateSchool(id, school);
        if(updatedSchool == null)
        {
            return NotFound(new { status = "error", message = "School not found" });
        }
        return Ok(updatedSchool);
    }
    
    [HttpDelete("{id}")]
    public ActionResult DeleteSchool(int id)
    {
        var school = _schoolService.GetSchool(id);
        if (school == null)
        {
            return NotFound(new { status = "error", message = "School not found" });
        }
        _schoolService.DeleteSchool(id);
        return NoContent();
    }
    
    [HttpGet("{schoolId}/detail")]
    public ActionResult<IEnumerable<SchoolStudentViewModel>> GetStudentDetailBySchool(int schoolId)
    {
        var schoolDetail = _schoolService.GetSchoolDetail(schoolId);
        if(schoolDetail == null)
        {
            return NotFound(new { status = "error", message = "School not found" });
        }
        return Ok(schoolDetail);

    }
}
