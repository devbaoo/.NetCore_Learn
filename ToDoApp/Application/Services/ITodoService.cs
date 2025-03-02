using ToDoApp.Domains.Entities;
using ToDoApp.Infrastructures;
using TodoWeb.Domains.Dtos;

namespace TodoApp.Application.Services
{
    public interface ITodoService
    {
        int Post(TodoCreateModel todo);
    }

    public class TodoService : ITodoService
    {
        private readonly IApplicationDBContext _dbContext;

        public TodoService(IApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Post(TodoCreateModel todo)
        {
            var data = new ToDo
            {
                Description = todo.Description
            };

            _dbContext.ToDos.Add(data);
            _dbContext.SaveChanges();
            return data.Id;
        }
    }
}