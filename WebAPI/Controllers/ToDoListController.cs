using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        ContextToDo contextToDo = new ContextToDo();

        [HttpGet("ToDoList")]
        public IEnumerable<ToDoList> Get()
        {

            return contextToDo.ToDoList;
        }

        [HttpGet("ToDoList/{todoUserID}&{todoState}")]
        public IEnumerable<ToDoList> ToDoGetByIDAndState(int todoUserID, int todoState)
        {
            var toDoList = contextToDo.ToDoList.Where(t => t.todoUserID == todoUserID && t.todoState == todoState).ToList();
            return toDoList;
        }

        [HttpPost("ToDoList")]
        public IEnumerable<ToDoList> Post([FromBody] ToDoList toDoList)
        {
            toDoList.todoSaveDate = DateTime.Now;
            contextToDo.ToDoList.Add(toDoList);
            contextToDo.SaveChanges();
            return contextToDo.ToDoList;
        }

        [HttpPut("UpdateTodo/{todoID}")]
        public async Task<IActionResult> UpdateTodoItem(int todoID, ToDoList toDoList)
        {   
            if (todoID != toDoList.todoID)
            {
                return BadRequest();
            }

            var toDoListItem = await contextToDo.ToDoList.FindAsync(todoID);
            if (toDoListItem == null)
            {
                return NotFound();
            }

            toDoListItem.todoState = toDoList.todoState;

            try
            {
                await contextToDo.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException) when (!ToDoListExists(todoID))
            {
                return NotFound();
            }

            return Ok(toDoList);
        }
        private bool ToDoListExists(int id)
        {
            return contextToDo.ToDoList.Any(t => t.todoID == id);
        }
    }
}
