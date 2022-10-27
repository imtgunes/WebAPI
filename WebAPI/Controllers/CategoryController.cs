using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        ContextToDo contextToDo = new ContextToDo();

        [HttpGet("Category")]
        public IEnumerable<Category> Get()
        {

            return contextToDo.Category;
        }

        [HttpPost("Category")]
        public IEnumerable<Category> Post([FromBody] Category category)
        {
            contextToDo.Category.Add(category);
            contextToDo.SaveChanges();
            return contextToDo.Category;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var categoryItem = await contextToDo.Category.FindAsync(id);
            if (categoryItem == null)
            {
                return NotFound();
            }

            contextToDo.Category.Remove(categoryItem);
            await contextToDo.SaveChangesAsync();

            return NoContent();
        }
    }
}
