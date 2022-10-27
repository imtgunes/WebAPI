namespace WebAPI.Models
{
    public class ShoppingCart
    {
        public int shoppingCartID { get; set; }
        public string? shoppingCartNote { get; set; }
        public int shoppingCartState { get; set; }
        public int shoppingCartUserID { get; set; }

    }
}
