using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApp.Application.Services;
using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures;
using TodoApp.Application.Services;

namespace ToDoApp.Controllers
{
    // GET, POST, PUT, DELETE
    // Idempotency: GET, PUT, DELETE
    // Non-Idempotency: POST

    //Example
    // POST request: create 1 row Todo 
    // When calling { Description: "Study", IsCompleted: false } 10 times => Create 10 rows in database

    // PUT request
    // Update 1 row Todo
    // When calling { Id = 1, Description: "Study", IsCompleted: false } 10 times

    // Client ---> Server ---> Database: OK; Server ---> Client: OK
    
    [ApiController]
    [Route("[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly IApplicationDBContext _dbContext;
        private readonly ITodoService _todoService;
        private readonly IGuidGenerator _guidGenerator;
        public TodoController(IApplicationDBContext dbContext, 
            ITodoService todoService, IGuidGenerator guidGenerator)
        { 
            _dbContext = dbContext;
            _todoService = todoService;
            _guidGenerator = guidGenerator;
        }
        [HttpGet("guid")]
        public Guid GetGuid()
        {
            return _guidGenerator.Generate();
        }

        [HttpGet]
        public IEnumerable<ToDo> Get(bool isCompleted) 
        { 
            //SELECT * FROM ToDos WHERE IsCompleted = isCompleted
            return _dbContext.ToDos
                .Where(x => x.IsCompleted == isCompleted)
                .ToList();
        }

        [HttpPost]
        public int Post(ToDo todo)
        {
            _dbContext.ToDos.Add(todo);

            _dbContext.SaveChanges();

            return todo.Id;
        }

        [HttpPut]
        public int Put(ToDo todo)
        {
            var data = _dbContext.ToDos.Find(todo.Id);
            if (data == null) return -1;

            data.Description = todo.Description;
            data.IsCompleted = todo.IsCompleted;

            _dbContext.SaveChanges();

            return todo.Id;
        }

        [HttpDelete]
        public void Delete(int id)
        {
            var data = _dbContext.ToDos.Find(id);
            if (data == null) return;

            _dbContext.ToDos.Remove(data);
            _dbContext.SaveChanges();
        }
    }
}
