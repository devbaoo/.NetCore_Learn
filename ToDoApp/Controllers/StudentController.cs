using Microsoft.AspNetCore.Mvc;
using ToDoApp.Application.Dtos;
using ToDoApp.Application.Services;
using ToDoApp.Domains.Entities;
using System.Collections.Generic;
using TodoApp.Application.Dtos;
using ToDoApp.Application.Dtos.StudentModel;

namespace ToDoApp.Controllers;

[Route("[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
        
    public StudentController(IStudentService studentService)
    {
        _studentService = studentService;
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<StudentViewModel>> GetStudents([FromQuery] int? schoolId)
    {
        var students = _studentService.GetStudents(schoolId);
        return Ok( students );
    }
    
    [HttpPost]
    public ActionResult<Student> CreateStudent([FromBody] StudentCreateModel student)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        
        var createdStudent = _studentService.CreateStudent(student);
        return Ok(createdStudent);
    }
    
    [HttpPut("{id}")]
    public ActionResult<Student> UpdateStudent(int id, [FromBody] StudentUpdateModel student)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = "error", message = "Invalid data", errors = ModelState });
        }
        
        var updatedStudent = _studentService.UpdateStudent(id, student);
        if(updatedStudent == null)
        {
            return NotFound(new { status = "error", message = "Student not found" });
        }
        
        return Ok(updatedStudent );
    }
    
    [HttpDelete("{id}")]
    public ActionResult DeleteStudent(int id)
    {
        var studentDetail = _studentService.GetStudentDetail(id);
        if (studentDetail == null)
        {
            return NotFound(new { status = "error", message = "Student not found" });
        }
        _studentService.DeleteStudent(id);
        return NoContent();
    }
    
    [HttpGet("{id}")]
    public ActionResult<StudentCourseViewModel> GetStudentDetail(int id)
    {
        var studentDetail = _studentService.GetStudentDetail(id);
        if(studentDetail == null)
        {
            return NotFound(new { status = "error", message = "Student not found" });
        }
        return Ok(studentDetail );
    }
    [HttpPatch("{id}/enroll")]
    public ActionResult EnrollCourse(int id, [FromBody] EnrollCourseModel model)
    {
        try
        {
            _studentService.EnrollCourse(id, model.CourseId);
            return Ok(new { status = "success", message = "Student enrolled in course successfully" });
        }
        catch (Exception ex)
        {
            return NotFound(new { status = "error", message = ex.Message });
        }
    }
    [HttpPost ("updateScore")]
    public ActionResult UpdateScore([FromBody] StudentScore model){
        try
        {
            _studentService.UpdateScore(model);
            return Ok(model);
        }
        catch (Exception ex)
        {
            return NotFound(new { status = "error", message = ex.Message });
        }
    }
    [HttpGet("{id}/averageScores")]
    public ActionResult<IEnumerable<StudentScore>>  GetStudentAverageScore(int id)
    {
        var averageScores = _studentService.GetStudentAverageScore(id);
        return Ok(averageScores);
    } 
    
    

}
