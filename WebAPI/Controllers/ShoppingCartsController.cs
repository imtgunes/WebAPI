using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Context;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        ContextToDo contextToDo = new ContextToDo();

        [HttpGet("ShoppingCarts")]
        public IEnumerable<ShoppingCarts> Get(int cartID)
        {
            var result = contextToDo.ShoppingCarts
                .Where(p => p.shoppingCartsCartID == cartID)
                .Join(contextToDo.Product,                                      
                     p => new { p1 = p.productID },     
                     e => new { p1 = e.productID },        
                      (p, e) => new ShoppingCarts
                      {                                 
                          Product = e,
                          shoppingCartsID = p.shoppingCartsID,
                          shoppingCartsCartID = p.shoppingCartsCartID,
                          shoppingCartsNote = p.shoppingCartsNote,
                          shoppingCartsPiece = p.shoppingCartsPiece,
                          productID = p.productID
                      }
                     )
       
                .ToList();
            return result;
        }

        [HttpGet("ShoppingCartsCart")]
        public IEnumerable<ShoppingCarts> GetbyShoppingCart(int scID)
        {
            return contextToDo.ShoppingCarts.Where(s => s.shoppingCartsCartID == scID).ToList();
        }

        [HttpPost("ShoppingCarts")]
        public IEnumerable<ShoppingCarts> Post([FromBody] ShoppingCarts shoppingCarts)
        {
            var s = new ShoppingCarts
            {
                shoppingCartsCartID=shoppingCarts.shoppingCartsCartID,
                productID = shoppingCarts.productID,
                shoppingCartsNote=shoppingCarts.shoppingCartsNote,
                shoppingCartsPiece=shoppingCarts.shoppingCartsPiece,
                Product =null
            };
            if (contextToDo.ShoppingCarts.Any(s => s.shoppingCartsCartID == shoppingCarts.shoppingCartsCartID && s.productID == shoppingCarts.productID))
            {

                int shoppingCartsID = contextToDo.ShoppingCarts.Where(s => s.shoppingCartsCartID == shoppingCarts.shoppingCartsCartID && s.productID == shoppingCarts.productID).Select(s => s.shoppingCartsID).SingleOrDefault();
                ShoppingCarts shoppingCartsExisting = new ShoppingCarts();
                shoppingCartsExisting = contextToDo.ShoppingCarts.Find(shoppingCartsID);

                ShoppingCarts shoppingCartsNew = new ShoppingCarts();

                shoppingCartsNew.shoppingCartsID = shoppingCartsExisting.shoppingCartsID;
                shoppingCartsNew.shoppingCartsCartID = shoppingCartsExisting.shoppingCartsCartID;
                shoppingCartsNew.productID = shoppingCartsExisting.productID;
                shoppingCartsNew.shoppingCartsNote = shoppingCarts.shoppingCartsNote;
               
                shoppingCartsNew.shoppingCartsPiece = shoppingCartsExisting.shoppingCartsPiece + shoppingCarts.shoppingCartsPiece;

                UpdateShoppingCarts(shoppingCartsID, shoppingCartsNew);

                return contextToDo.ShoppingCarts;
            }
            contextToDo.ShoppingCarts.Add(s);
            contextToDo.SaveChanges();
            return contextToDo.ShoppingCarts;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShoppingCarts(int id)
        {
            var shoppingCartsItem = await contextToDo.ShoppingCarts.FindAsync(id);
            if (shoppingCartsItem == null)
            {
                return NotFound();
            }

            contextToDo.ShoppingCarts.Remove(shoppingCartsItem);
            await contextToDo.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateShoppingCarts(int id, ShoppingCarts shoppingCarts)
        {
            if (id != shoppingCarts.shoppingCartsID)
            {
                return BadRequest();
            }

            var shoppingCartsItem = contextToDo.ShoppingCarts.Find(id);
            if (shoppingCartsItem == null)
            {
                return NotFound();
            }

            shoppingCartsItem.shoppingCartsPiece = shoppingCarts.shoppingCartsPiece;
            shoppingCartsItem.shoppingCartsNote = shoppingCarts.shoppingCartsNote;

            try
            {
                contextToDo.SaveChanges();
                return Ok(shoppingCarts);
            }
            catch (DbUpdateConcurrencyException) when (!ShoppingCartsExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }
        private bool ShoppingCartsExists(int id)
        {
            return contextToDo.ShoppingCarts.Any(s => s.shoppingCartsID == id);
        }

    }
}
