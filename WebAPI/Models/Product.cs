using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Product
    {
        [Key]
        public int productID { get; set; }
        public string productName { get; set; }

        public int productCategoryID { get; set; }

        public string productImage { get; set; }

        public float productWeight { get; set; }

    }
}
