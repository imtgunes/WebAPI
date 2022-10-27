using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        ContextToDo contextToDo = new ContextToDo();

        [HttpGet("ShoppingCart")]
        public IEnumerable<ShoppingCart> GetNote(int shoppingCartID)
        {
            return contextToDo.ShoppingCart.Where(s => s.shoppingCartID == shoppingCartID);
        }

        [HttpGet("ShoppingCartState")]
        public int GetShoppingCartState(int userID)
        {
            var shoppingCartItem = contextToDo.ShoppingCart.Where(sc => sc.shoppingCartUserID == userID && sc.shoppingCartState == 0).Select(sc => sc.shoppingCartID).SingleOrDefault();
            if (shoppingCartItem == 0)
            {
                ShoppingCart shoppingCart = new ShoppingCart(); 
                shoppingCart.shoppingCartUserID = userID;
                shoppingCart.shoppingCartState = 0;
                Post(shoppingCart);
                shoppingCartItem = contextToDo.ShoppingCart.Where(sc => sc.shoppingCartUserID == userID && sc.shoppingCartState == 0).Select(sc => sc.shoppingCartID).SingleOrDefault();
                return shoppingCartItem;
            }
            return shoppingCartItem;
        }

        [HttpPost("ShoppingCart")]
        public IEnumerable<ShoppingCart> Post([FromBody] ShoppingCart shoppingCart)
        {
            contextToDo.ShoppingCart.Add(shoppingCart);
            contextToDo.SaveChanges();
            return contextToDo.ShoppingCart;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingCart(int id)
        {
            var shoppingCartItem = await contextToDo.ShoppingCart.FindAsync(id);
            if (shoppingCartItem == null)
            {
                return NotFound();
            }

            contextToDo.ShoppingCart.Remove(shoppingCartItem);
            await contextToDo.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShoppingCart(int id, ShoppingCart shoppingCart)
        {
            if (id != shoppingCart.shoppingCartID)
            {
                return BadRequest();
            }

            var shoppingCartItem = await contextToDo.ShoppingCart.FindAsync(id);
            if (shoppingCartItem == null)
            {
                return NotFound();
            }

            shoppingCartItem.shoppingCartNote = shoppingCart.shoppingCartNote;
            shoppingCartItem.shoppingCartState = shoppingCart.shoppingCartState;

            try
            {
                await contextToDo.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ShoppingCartExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        private bool ShoppingCartExists(int id)
        {
            return contextToDo.ShoppingCart.Any(s => s.shoppingCartID == id);
        }
    }
}
