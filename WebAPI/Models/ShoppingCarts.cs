using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebAPI.Models
{
    public class ShoppingCarts
    {
        [Key]
        public int shoppingCartsID { get; set; }
        public int shoppingCartsCartID { get; set; }
        public int shoppingCartsPiece { get; set; }
        public string? shoppingCartsNote { get; set; }

        [ForeignKey("Product")]
        public int productID { get; set; }
        public virtual Product? Product { get; set; }
    }
}
