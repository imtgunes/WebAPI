using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        ContextToDo contextToDo = new ContextToDo();

        [HttpGet("Products")]
        public IEnumerable<Product> Get()
        {
            return contextToDo.Product;
        }

        [HttpGet("ProductsByID")]
        public IEnumerable<Product> GetProductsByID(int productID)
        {

            return contextToDo.Product.Where(pbi => pbi.productID == productID).ToList();
        }

        [HttpGet("ProductsByCategory")]
        public IEnumerable<Product> GetProductsByCategory(int category)
        {
            return contextToDo.Product.Where(pbc => pbc.productCategoryID == category).ToList();
        }

        [HttpPost("Products")]
        public IEnumerable<Product> Post([FromBody] Product product)
        {
            contextToDo.Product.Add(product);
            contextToDo.SaveChanges();
            return contextToDo.Product;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var productItem = await contextToDo.Product.FindAsync(id);
            if (productItem == null)
            {
                return NotFound();
            }

            contextToDo.Product.Remove(productItem);
            await contextToDo.SaveChangesAsync();

            return NoContent();
        }
    }
}
