using Microsoft.AspNetCore.Mvc;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Dtos;
using ToDoApp.Application.Services;
using ToDoApp.Domains.Entities;

namespace ToDoApp.Controllers;


[Route("[controller]")]
[ApiController]
public class CourseController: ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CourseViewModel>> GetCourses()
    {
        var courses = _courseService.GetCourses();
        return Ok( courses );
    }

    [HttpGet("{id}")]
    public ActionResult<Course> GetCourse(int id)
    {
        var course = _courseService.GetCourse(id);
        if (course == null)
        {
            return NotFound(new { status = "error", message = "Course not found" });
        }

        return Ok(course );
    }

    [HttpPost]
    public ActionResult<Course> CreateCourse([FromBody] CourseCreateModel course)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }

        var createdCourse = _courseService.CreateCourse(course);
        return CreatedAtAction(nameof(GetCourse), new { id = createdCourse.Id },
            new {  createdCourse });
    }

    [HttpPut("{id}")]
    public ActionResult<Course> UpdateCourse(int id, [FromBody] CourseUpdateModel course)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }

        var updatedCourse = _courseService.UpdateCourse(id, course);
        if (updatedCourse == null)
        {
            return NotFound(new { status = "error", message = "Course not found" });
        }

        return Ok(updatedCourse );
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteCourse(int id)
    {
        var course = _courseService.GetCourse(id);
        if (course == null)
        {
            return NotFound(new { status = "error", message = "Course not found" });
        }

        _courseService.DeleteCourse(id);
        return NoContent();
    }
    
    [HttpGet("{id}/students")]
    public ActionResult<CourseStudentViewModel> GetCourseDetail(int id)
    {
        var courseDetail = _courseService.GetCourseDetail(id);
        if(courseDetail == null)
        {
            return NotFound(new { status = "error", message = "Student not found" });
        }
        return Ok(courseDetail );
    }
     
}