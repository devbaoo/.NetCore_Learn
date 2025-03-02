using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Dtos;
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
    public ActionResult<IEnumerable<SchoolViewModel>> GetSchools()
    {
        var schools = _schoolService.GetSchools();
        return Ok(schools);
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
        return CreatedAtAction(nameof(GetSchool), new { id = createdSchool.Id }, 
            new { createdSchool });
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
}
